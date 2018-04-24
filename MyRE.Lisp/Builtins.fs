module Builtins

open MyRE.Lisp
open MyRE.Lisp.Types
open MyRE.Lisp.Node
open MyRE.Lisp.Core

let core eval = [
                    ("+", Core.add);
                    ("-", Core.subtract);
                    ("*", Core.multiply);
                    ("/", Core.divide);
                    ("list", Core.list);
                    ("list?", Core.isList);
                    ("empty?", Core.isEmpty);
                    ("count", Core.count);
                    ("=", Core.eq);
                    ("<", Core.lt);
                    ("<=", Core.le);
                    (">=", Core.ge);
                    (">", Core.gt);
                    ("time-ms", Core.time_ms);
                    ("pr-str", Core.pr_str);
                    ("str", Core.str);
                    ("prn", Core.prn);
                    ("println", Core.println);
                    ("read-string", Core.read_str);
                    ("slurp", Core.slurp);
                    ("cons", Core.cons);
                    ("concat", Core.concat);
                    ("nth", Core.nth);
                    ("first", Core.first);
                    ("rest", Core.rest);
                    ("throw", Core.throw);
                    ("map", Core.map eval);
                    ("apply", Core.apply eval);
                    ("nil?", (Core.isConst Node.NIL));
                    ("true?", (Core.isConst Node.TRUE));
                    ("false?", (Core.isConst Node.FALSE));
                    ("symbol?", Core.isSymbol);
                    ("symbol", Core.symbol);
                    ("string?", Core.isString);
                    ("keyword?", Core.isKeyword);
                    ("keyword", Core.keyword);
                    ("number?", Core.isNumber);
                    ("fn?", Core.isFn);
                    ("macro?", Core.isMacro);
                    ("sequential?", Core.isSequential);
                    ("vector?", Core.isVector);
                    ("vector", Core.vector);
                    ("map?", Core.isMap);
                    ("hash-map", Core.hashMap);
                    ("assoc", Core.assoc);
                    ("dissoc", Core.dissoc);
                    ("get", Core.get);
                    ("contains?", Core.contains);
                    ("keys", Core.keys);
                    ("vals", Core.vals);
                    ("atom", Core.atom);
                    ("atom?", Core.isAtom);
                    ("deref", Core.deref);
                    ("reset", Core.reset);
                    ("swap", Core.swap eval);
                    ("conj", Core.conj);
                    ("seq", Core.seq);
                    ("meta", Core.meta);
                    ("with-meta", Core.withMeta );] 
                |> Map.ofList

let def eval env = function
    | [sym; form] ->
        match sym with
        | Symbol(sym) ->
            let node = eval env form
            Env.set env sym node
            node
        | _ -> raise <| Error.errExpectedX "symbol"
    | _ -> raise <| Error.wrongArity ()

let defineMacro eval env = function
    | [sym; form] ->
        match sym with
        | Symbol(sym) ->
            let node = eval env form
            match node with
            | Func(_, _, body, binds, outer) ->
                let node = Env.makeMacro body binds outer
                Env.set env sym node
                node
            | _ -> raise <| Error.errExpectedX "user defined func"
        | _ -> raise <| Error.errExpectedX "symbol"
    | _ -> raise <| Error.wrongArity ()

let rec macroExpand eval env = function
    | Env.IsMacro env (m, rest) ->
        Core.callFunction eval m rest |> macroExpand eval env
    | node -> node

let handleMacroExpand eval env =  function
    | [form] -> macroExpand eval env form
    | _ -> raise <| Error.wrongArity ()

let setBinding eval env first second =
    let s = match first with
            | Symbol(s) -> s
            | _ -> raise <| Error.errExpectedX "symbol"
    let form = eval env second
    Env.set env s form

let ``let`` eval outer = function
    | [bindings; form] ->
        let inner = Env.makeNew outer [] []
        let binder = setBinding eval inner
        match bindings with
        | List(_) | Vector(_) -> iterPairs binder bindings
        | _ -> raise <| Error.errExpectedX "list or vector"
        inner, form
    | _ -> raise <| Error.wrongArity ()

let ifThen eval env condForm trueForm falseForm =
    match eval env condForm with
    | Bool(false) | Nil -> falseForm
    | _ -> trueForm

let ``if`` eval env = function
    | [condForm; trueForm; falseForm] -> ifThen eval env condForm trueForm falseForm
    | [condForm; trueForm] -> ifThen eval env condForm trueForm Nil
    | _ -> raise <| Error.wrongArity ()

let rec ``do`` eval env = function
    | [a] -> a
    | a::rest ->
        eval env a |> ignore
        ``do`` eval env rest
    | _ -> raise <| Error.wrongArity ()

let fn outer nodes =
    let makeFunc binds body =
        Env.makeFunc body binds outer

    match nodes with
    | [List(_, binds); body] -> makeFunc binds body
    | [Vector(_, seg); body] -> makeFunc (List.ofSeq seg) body
    | [_; _] -> raise <| Error.errExpectedX "bindings of list or vector"
    | _ -> raise <| Error.wrongArity ()

let catch eval env err = function
    | List(_, [Symbol("catch"); Symbol(_) as sym; catchBody]) ->
        let inner = Env.makeNew env [sym] [err]
        catchBody |> eval inner
    | List(_, [_; _; _]) -> raise <| Error.argMismatch ()
    | _ -> raise <| Error.wrongArity ()

let ``try`` eval env = function
    | [exp; catchClause] ->
        try
            eval env exp
        with
        | Error.EvalError(str)
        | Error.ReaderError(str) -> catch eval env (String(str)) catchClause
        | Error.MyreLispException(node) -> catch eval env node catchClause
    | _ -> raise <| Error.wrongArity ()

let quote = function
    | [node] -> node
    | _ -> raise <| Error.wrongArity ()

let quasiquote nodes =
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

let eval _eval env = function
    | [ast] -> _eval env ast
    | _ -> raise <| Error.wrongArity ()
