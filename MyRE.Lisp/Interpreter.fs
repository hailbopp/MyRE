module MyRE.Lisp.Interpreter

open System
open Node
open Types

open MyRE.SmartApp.Api.Client
open MyRE.SmartApp.Api.Client.Models
open System.Threading.Tasks

type ExecutionContext = {
    smartAppClient: IMyreSmartAppApiClient;
}

type InterpreterState = { 
    input: string;
    output: Node seq;
    printed: string seq;
    env: Env list;
}

type Interpreter = {
    REPL: string array -> int;
    REYL: (unit -> string) -> InterpreterState option seq;
}

let createInterpreter (execContext: ExecutionContext) =
    let handleSmartThingsResponse (res: ApiResponse<'a>) =
        match toNativeOption res.Error with
        | Option.Some x -> Error.smartThingsFailure x.Message |> raise
        | Option.None -> res.Data

    let makeSmartAppApiRequest (makeReq: unit -> 'a ApiResponse Task) =
        async { return! makeReq() |> Async.AwaitTask } |> Async.RunSynchronously |> handleSmartThingsResponse

    let getAllDeviceInfo () = (fun () -> execContext.smartAppClient.ListDevicesAsync()) |> makeSmartAppApiRequest
        
    let getDeviceState = function
        | DeviceReference(devId) ->  
            makeSmartAppApiRequest (fun () -> execContext.smartAppClient.GetDeviceStatusAsync(devId))
        | _ -> Error.argMismatch() |> raise


    let rec iterPairs f = function
        | Pair(first, second, t) ->
            f first second
            iterPairs f t
        | Empty -> ()
        | _ -> raise <| Error.errExpectedX "list or vector"

    let quasiquoteForm nodes =
        let transformNode f = function
            | Elements 1 [|a|] -> f a
            | _ -> raise <| Error.wrongArity ()
        let singleNode = transformNode (fun n -> n)
        let rec quasiquote node =
            match node with
            | Cons(Symbol("unquote"), rest) -> rest |> singleNode
            | Cons(Cons(Symbol("splice-unquote"), spliceRest), rest) ->
                makeList [Symbol("concat"); singleNode spliceRest; quasiquote rest]
            | Cons(h, t) -> makeList [Symbol("cons"); quasiquote h; quasiquote t]
            | n -> makeList [Symbol("quote"); n]
        makeList nodes |> transformNode quasiquote

    let quoteForm = function
        | [node] -> node
        | _ -> raise <| Error.wrongArity ()

    let rec macroExpand env = function
        | Env.IsMacro env (Macro(_, _, f, _, _, _), rest) ->
            f rest |> macroExpand env
        | node -> node

    let rec eval_ast env = function
        | Symbol(sym) -> Env.get env sym
        | List(_, lst) -> lst |> List.map (eval env) |> makeList
        | Vector(_, seg) -> seg |> Seq.map (eval env) |> Array.ofSeq |> Node.ofArray
        | Map(_, map) -> map |> Map.map (fun k v -> eval env v) |> makeMap
        | node -> node

    and defBangForm env = function
        | [sym; form] ->
            match sym with
            | Symbol(sym) ->
                let node = eval env form
                Env.set env sym node
                node
            | _ -> raise <| Error.errExpectedX "symbol"
        | _ -> raise <| Error.wrongArity ()

    and defMacroForm env = function
        | [sym; form] ->
            match sym with
            | Symbol(sym) ->
                let node = eval env form
                match node with
                | Func(_, _, f, body, binds, outer) ->
                    let node = Env.makeMacro f body binds outer
                    Env.set env sym node
                    node
                | _ -> raise <| Error.errExpectedX "user defined func"
            | _ -> raise <| Error.errExpectedX "symbol"
        | _ -> raise <| Error.wrongArity ()

    and macroExpandForm env = function
        | [form] -> macroExpand env form
        | _ -> raise <| Error.wrongArity ()

    and setBinding env first second =
        let s = match first with
                | Symbol(s) -> s
                | _ -> raise <| Error.errExpectedX "symbol"
        let form = eval env second
        Env.set env s form

    and letStarForm outer = function
        | [bindings; form] ->
            let inner = Env.makeNew outer [] []
            let binder = setBinding inner
            match bindings with
            | List(_) | Vector(_) -> iterPairs binder bindings
            | _ -> raise <| Error.errExpectedX "list or vector"
            inner, form
        | _ -> raise <| Error.wrongArity ()

    and ifForm env = function
        | [condForm; trueForm; falseForm] -> ifForm3 env condForm trueForm falseForm
        | [condForm; trueForm] -> ifForm3 env condForm trueForm Nil
        | _ -> raise <| Error.wrongArity ()

    and ifForm3 env condForm trueForm falseForm =
        match eval env condForm with
        | Bool(false) | Nil -> falseForm
        | _ -> trueForm

    and doForm env = function
        | [a] -> a
        | a::rest ->
            eval env a |> ignore
            doForm env rest
        | _ -> raise <| Error.wrongArity ()

    and fnStarForm outer nodes =
        let makeFunc binds body =
            let f = fun nodes ->
                        let inner = Env.makeNew outer binds nodes
                        eval inner body
            Env.makeFunc f body binds outer

        match nodes with
        | [List(_, binds); body] -> makeFunc binds body
        | [Vector(_, seg); body] -> makeFunc (List.ofSeq seg) body
        | [_; _] -> raise <| Error.errExpectedX "bindings of list or vector"
        | _ -> raise <| Error.wrongArity ()

    and catchForm env err = function
        | List(_, [Symbol("catch"); Symbol(_) as sym; catchBody]) ->
            let inner = Env.makeNew env [sym] [err]
            catchBody |> eval inner
        | List(_, [_; _; _]) -> raise <| Error.argMismatch ()
        | _ -> raise <| Error.wrongArity ()

    and tryForm env = function
        | [exp; catchClause] ->
            try
                eval env exp
            with
            | Error.EvalError(str)
            | Error.ReaderError(str) -> catchForm env (String(str)) catchClause
            | Error.MyreLispException(node) -> catchForm env node catchClause
        | _ -> raise <| Error.wrongArity ()

    and getDevicesForm env = function
        | [] -> 
            getAllDeviceInfo()
             |> Seq.map (fun (di) -> DeviceReference(di.DeviceId)) |> List.ofSeq
                              |> makeList
        | _ -> raise <| Error.wrongArity ()

    and devForm env = function
        | [String(devIdentifier);] -> 
            getAllDeviceInfo()
                |> Seq.filter (fun di -> di.DeviceId = devIdentifier || di.Label = devIdentifier)
                |> Seq.map (fun di -> di.DeviceId)
                |> Seq.head
                |> DeviceReference

        | _ -> raise <| Error.wrongArity ()

    and usingDevForm env = function
        | [Symbol(sym); DeviceReference(ref); body;] -> Nil //TODO
        | _ -> raise <| Error.wrongArity ()

    and eval env = function
        | List(_, _) as node ->
            match macroExpand env node with
            | List(_, []) as emptyList -> emptyList
            | List(_, Symbol("def")::rest) -> defBangForm env rest
            | List(_, Symbol("defmacro")::rest) -> defMacroForm env rest
            | List(_, Symbol("macroexpand")::rest) -> macroExpandForm env rest
            | List(_, Symbol("let")::rest) ->
                let inner, form = letStarForm env rest
                form |> eval inner
            | List(_, Symbol("if")::rest) -> ifForm env rest |> eval env
            | List(_, Symbol("do")::rest) -> doForm env rest |> eval env
            | List(_, Symbol("fn")::rest) -> fnStarForm env rest
            | List(_, Symbol("quote")::rest) -> quoteForm rest
            | List(_, Symbol("quasiquote")::rest) -> quasiquoteForm rest |> eval env
            | List(_, Symbol("try")::rest) -> tryForm env rest
            | List(_, Symbol("get-devices")::rest) -> getDevicesForm env rest
            | List(_, Symbol("dev")::rest) -> devForm env rest
            | List(_, Symbol("using-dev")::rest) -> usingDevForm env rest
            | List(_, _) as node ->
                let resolved = node |> eval_ast env
                match resolved with
                | List(_, BuiltInFunc(_, _, f)::rest) -> f rest
                | List(_, Func(_, _, _, body, binds, outer)::rest) ->
                    let inner = Env.makeNew outer binds rest
                    body |> eval inner
                | _ -> raise <| Error.errExpectedX "func"
            | node -> node |> eval_ast env
        | node -> node |> eval_ast env

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

    let RE env input =
        READ input
        |> Seq.ofList
        |> Seq.choose (fun form -> EVAL env form)

    let REP env input =
        input
        |> RE env
        |> Seq.iter (fun value -> PRINT value)

    let getReadlineMode args =
        if args |> Array.exists (fun e -> e = "--raw") then
            Reader.LineReadMode.RawInput
        else
            Reader.LineReadMode.Terminal

    let eval_func env = function
        | [ast] -> eval env ast
        | _ -> raise <| Error.wrongArity ()

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
        let env = Env.makeRootEnv ()

        Env.set env "eval" <| Env.makeBuiltInFunc (eval_func env)
        Env.set env "*ARGV*" <| argv_func args
        Env.set env "readline" <| Env.makeBuiltInFunc (readline_func mode)

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
    let REYL (readFunction: unit -> string) =
        let mode = Reader.RawInput
        let env = configureEnv List.empty mode

        let rep = ignore >> readFunction >> mapInterpreterState env

        Seq.initInfinite id
            |> Seq.map rep
            |> Seq.takeWhile Option.isSome

    { REPL = REPL; REYL = REYL }