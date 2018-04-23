module MyRE.Lisp.Env

    open Types
    open MyRE.SmartApp.Api.Client.Models

    let makeEmpty () = Env()

    let ofList lst =
        let env = makeEmpty ()
        let accumulate (e : Env) (k, v) = e.Add(k, v); e
        List.fold accumulate env lst

    let set (env : EnvChain) key node =
        match env with
        | head::_ -> head.[key] <- node
        | _ -> raise <| Error.noEnvironment ()

    let rec find (chain : EnvChain) key =
        match chain with
        | [] -> None
        | env::rest ->
            match env.TryGetValue(key) with
            | true, v -> Some(v)
            | false, _ -> find rest key

    let get chain key =
        match find chain key with
        | Some(v) -> v
        | None -> raise <| Error.symbolNotFound key

    let private getNextValue =
        let counter = ref 0
        fun () -> System.Threading.Interlocked.Increment(counter)

    let makeBuiltInFunc f =
        BuiltInFunc(Node.NIL, getNextValue (), f)

    let makeFunc f body binds env =
        Func(Node.NIL, getNextValue (), f, body, binds, env)

    let makeMacro f body binds env =
        Macro(Node.NIL, getNextValue (), f, body, binds, env)

    let makeRootEnv () =
        let wrap name f = name, makeBuiltInFunc f
        [[
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
            ("map", Core.map);
            ("apply", Core.apply);
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
            ("atom", (Core.atom getNextValue));
            ("atom?", Core.isAtom);
            ("deref", Core.deref);
            ("reset", Core.reset);
            ("swap", Core.swap);
            ("conj", Core.conj);
            ("seq", Core.seq);
            ("meta", Core.meta);
            ("with-meta", Core.withMeta );
        ]
        |> List.map (fun a -> a ||> wrap)
        |> ofList]

    let makeNew outer symbols nodes =
        let env = (makeEmpty ())::outer
        let rec loop symbols nodes =
            match symbols, nodes with
            | [Symbol("&"); Symbol(s)], nodes ->
                set env s (Node.makeList nodes)
                env
            | Symbol("&")::_, _ -> raise <| Error.onlyOneSymbolAfterAmp ()
            | Symbol(s)::symbols, n::nodes -> 
                set env s n
                loop symbols nodes
            | [], [] -> env
            | _, [] -> raise <| Error.notEnoughValues ()
            | [], _ -> raise <| Error.tooManyValues ()
            | _, _ -> raise <| Error.errExpectedX "symbol"
        loop symbols nodes

    (* Active Patterns to help with pattern matching nodes *)
    let inline (|IsMacro|_|) env = function
        | List(_, Symbol(sym)::rest) ->
            match find env sym with
            | Some(Macro(_, _, _, _, _, _) as m) -> Some(IsMacro m, rest)
            | _ -> None
        | _ -> None