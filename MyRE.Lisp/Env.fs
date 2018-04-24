module MyRE.Lisp.Env

    open Types
    open MyRE.SmartApp.Api.Client.Models
    open Newtonsoft.Json

    let makeEmpty () = Env()
    
    let makeEmptyChain () = [makeEmpty()]

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

    let private getNextValue =
        let counter = ref 0
        fun () -> System.Threading.Interlocked.Increment(counter)

    let makeAtom = function
        | [node] -> Atom((getNextValue ()), ref node)
        | _ -> Error.wrongArity() |> raise
        
    let deepCopy (envChain: EnvChain) =
        JsonConvert.DeserializeObject<EnvChain>(JsonConvert.SerializeObject(envChain));

    let makeFunc body binds env =
        Func(Node.NIL, getNextValue (), body, binds, deepCopy env)

    let makeMacro body binds env =
        Macro(Node.NIL, getNextValue (), body, binds, deepCopy env)

        
    //let rec copyNode = function
    //    | Nil -> Nil
    //    | List(md, items) -> List(md, List.map copyNode items)
    //    | Vector(md, items) -> Vector(md, System.ArraySegment(Array.map copyNode items.Array))
    //    | Map(md, items) -> Map(md, Map.toSeq items |> Seq.map (fun (k, v) -> copyNode k, copyNode v) |> Map.ofSeq)
    //    | Symbol(v) -> Symbol(v)
    //    | Keyword(v) -> Keyword(v)
    //    | Number(v) -> Number(v)
    //    | String(v) -> Node.String(v)
    //    | Bool(v) -> Bool(v)
    //    | BuiltinReference(n) -> BuiltinReference(n)
    //    | Func(md, uid, body, binds, envchain) -> Func(md, uid, copyNode body, List.map copyNode binds, deepCopy envchain)
    //    | Macro(md, uid, body, binds, envchain) -> Macro(md, uid, copyNode body, List.map copyNode binds, deepCopy envchain)
    //    | Atom(uid, v) -> Atom(uid, v)
    //    | DeviceReference(devId) -> DeviceReference(devId)
    //    | DeviceState(state) -> Node.DeviceState(state)

    //and copy (env: Env): Env =
    //    let newEnv = makeEmpty()

    //    Seq.map2 (fun k v -> k, v) env.Keys env.Values
    //    |> Seq.iter (fun (k, v) -> 
    //        newEnv.Add(k, copyNode v)
    //    )

    //    newEnv


    let makeNew outer symbols nodes =
        let env = (makeEmpty ())::(deepCopy outer)
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
            | Some(Macro(_, _, _, _, _) as m) -> Some(IsMacro m, rest)
            | _ -> None
        | _ -> None