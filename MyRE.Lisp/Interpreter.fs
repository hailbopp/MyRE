module MyRE.Lisp.Interpreter

open System
open Node
open Types

open MyRE.SmartApp.Api.Client
open MyRE.SmartApp.Api.Client.Models
open System.Threading.Tasks
open MyRE.Lisp.SmartApp

type InterpreterState = { 
    input: string;
    output: Node seq;
    printed: string seq;
    env: Env list;
}

type Interpreter = {
    REPL: string array -> int;
    REYL: InterpreterState option seq;
    readEvalOutputAll: (string) -> (Node * string seq * Env list) list;
}

let createInterpreter (smartAppClient: IMyreSmartAppApiClient) =
    let smartApp = SmartAppClient smartAppClient

    let log o = 
        printfn "%A" o
        o

    let rec eval_ast env = function
        | Symbol(sym) -> match Env.find env sym with
                         | Some(v) -> v
                         | None -> match getBuiltin sym with
                                    | Some(builtinFunc) -> BuiltinReference(sym)
                                    | None -> Error.symbolNotFound(sym) |> raise
        | List(_, lst) -> lst |> List.map (eval env) |> makeList
        | Vector(_, seg) -> seg |> Seq.map (eval env) |> Array.ofSeq |> Node.ofArray
        | Map(_, map) -> map |> Map.map (fun k v -> eval env v) |> makeMap
        | node -> node

    and eval env lst =
        match lst with
            | List(_, _) as node ->
                match Builtins.macroExpand eval env node with
                    | List(_, []) as emptyList -> emptyList
                    | List(_, Symbol("def")::rest) -> Builtins.def eval env rest
                    | List(_, Symbol("defmacro")::rest) -> Builtins.defineMacro eval env rest
                    | List(_, Symbol("macroexpand")::rest) -> Builtins.handleMacroExpand eval env rest
                    | List(_, Symbol("let")::rest) ->
                        let inner, form = Builtins.``let`` eval env rest
                        form |> eval inner
                    | List(_, Symbol("if")::rest) -> Builtins.``if`` eval env rest |> eval env
                    | List(_, Symbol("do")::rest) -> Builtins.``do`` eval env rest |> eval env
                    | List(_, Symbol("fn")::rest) -> Builtins.fn env rest
                    | List(_, Symbol("quote")::rest) -> Builtins.quote rest
                    | List(_, Symbol("quasiquote")::rest) -> Builtins.quasiquote rest |> eval env
                    | List(_, Symbol("try")::rest) -> Builtins.``try`` eval env rest
                    | List(_, Symbol("eval")::rest) -> Builtins.eval eval env rest

                    | List(_, Symbol("get-devices")::rest) -> smartApp.getAllDeviceReferences eval env rest
                    | List(_, Symbol("dev-ref")::rest) -> smartApp.getDeviceReference eval env rest
                    | List(_, Symbol("dev-state")::rest) -> smartApp.getDeviceState eval env rest

                    | List(_, _) as node ->
                        let resolved = eval_ast env node
                        match resolved with
                        | List(_, f::rest & Func(_, _, _, _, _)::_) ->
                            Core.callFunction eval f rest
                        | List(_, BuiltinReference(name)::rest) ->
                            Map.find name builtins <| rest
                        | _ -> raise <| Error.errExpectedX "func"
                    | node -> node |> eval_ast env
            | node -> node |> eval_ast env        
        

    and builtins = Builtins.core eval
    and getBuiltin = builtins.TryFind

    let READ input =
        try
            Reader.read_str input
        with
        | Error.ReaderError(msg) ->
            printfn "%s" msg
            []

    let EVAL env ast =
        try
            Some(eval env ast)
        with
        | Error.EvalError(str)
        | Error.ReaderError(str) ->
            printfn "%s" str
            None
        | Error.MyreLispException(node) ->
            printfn "%s" (Printer.pr_str [node])
            None
        | ex ->
            printfn "%s" (ex.Message)
            None

    let STRINGIFY = Seq.singleton >> Printer.pr_str    

    let PRINT = STRINGIFY >> printfn "%s"

    let R = READ
    let E env = Seq.ofList >> Seq.choose (fun form -> EVAL env form)        

    let RE env input = R input |> E env        

    let P = Seq.iter (fun value -> PRINT value)

    let REP env input =
        input
        |> RE env
        |> P

    let getReadlineMode args =
        if args |> Array.exists (fun e -> e = "--raw") then
            Reader.LineReadMode.RawInput
        else
            Reader.LineReadMode.Terminal            

    let argv_func = function
        | file::rest -> rest |> List.map Types.String |> makeList
        | [] -> EmptyLIST

    let readline_func mode = function
        | [String(prompt)] ->
            match Reader.read prompt mode with
            | null -> Node.NIL
            | input -> String(input)
        | [_] -> raise <| Error.argMismatch ()
        | _ -> raise <| Error.wrongArity ()

    let configureEnv args mode =
        let env = Env.makeEmptyChain()

        //Env.set env "eval" <| Env.makeBuiltInFunc (eval_func env)
        //Env.set env "*ARGV*" <| argv_func args
        //Env.set env "readline" <| Env.makeBuiltInFunc (readline_func mode)

        RE env """
            (def not (fn (a) (if a false true)))
            (def load-file (fn (f) (eval (read-string (slurp f)))))
            (defmacro cond (fn (& xs) (if (> (count xs) 0) (list 'if (first xs) (if (> (count xs) 1) (nth xs 1) (throw "odd number of forms to cond")) (cons 'cond (rest (rest xs)))))))
            (def *gensym-counter* (atom 0))
            (def gensym (fn [] (symbol (str "G__" (swap *gensym-counter* (fn [x] (+ 1 x)))))))
            (defmacro or (fn (& xs) (if (empty? xs) nil (if (= 1 (count xs)) (first xs) (let (condvar (gensym)) `(let (~condvar ~(first xs)) (if ~condvar ~condvar (or ~@(rest xs)))))))))
            """ |> Seq.iter ignore

        env

    let REPL args =
        let mode = getReadlineMode args
        let args = Seq.ofArray args |> Seq.filter (fun e -> e <> "--raw") |> List.ofSeq
        let env = configureEnv args mode

        match args with
        | file::_ ->
            System.IO.File.ReadAllText file
            |> RE env |> Seq.iter ignore
            0
        | _ ->
            let rec loop () =
                match Reader.read "user> " mode with
                | null -> 0
                | input ->
                    REP env input
                    loop ()
            loop ()

    let mapInterpreterState env input =
        match String.IsNullOrWhiteSpace input with
        | true -> None
        | false -> 
            // We want the env to be mutated first -- there's a TODO here around making Env immutable
            let out = RE env input
            Some { input = input; output = out; printed = Seq.map STRINGIFY out; env = env }

    // REYL: READ-EVAL-YIELD-LOOP
    let REYL =
        let env = configureEnv List.empty Reader.RawInput

        let rep = ignore >> Console.ReadLine >> mapInterpreterState env

        Seq.initInfinite id
            |> Seq.map rep
            |> Seq.takeWhile Option.isSome

    let readEvalOutputAll src =
        let mode = Reader.RawInput
        let env = configureEnv List.empty mode

        let parsedInputs = R src
        let outputs = 
            parsedInputs
                |> List.map (fun i -> E env [i])

        List.map2 (fun a b -> a, Seq.map STRINGIFY b, env) parsedInputs outputs



    { REPL = REPL; REYL = REYL; readEvalOutputAll = readEvalOutputAll }