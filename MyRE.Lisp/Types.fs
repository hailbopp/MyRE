module MyRE.Lisp.Types

open MyRE.SmartApp.Api.Client.Models

type NumberType = decimal
type DeviceId = string

[<CustomEquality; CustomComparison>]
type Node =
    | Nil
    | List of meta: Metadata * value: Node list
    | Vector of meta: Metadata * value: Node System.ArraySegment
    | Map of meta: Metadata * value: Collections.Map<Node, Node>
    | Symbol of value: string
    | Keyword of value: string
    | Number of value: NumberType
    | String of value: string
    | Bool of value: bool
    | BuiltinReference of value: string
    | Func of meta: Metadata * uid: int * body: Node * binds: Node list * env: EnvChain
    | Macro of meta: Metadata * uid: int * body: Node * binds: Node list * env: EnvChain
    | Atom of uid: int * value: Node Ref
    | DeviceReference of deviceId: DeviceId * label: string
    | DeviceState of deviceId: DeviceId * state: Node

    static member private hashSeq (s : seq<Node>) =
        let iter st node = (st * 397) ^^^ node.GetHashCode()
        s |> Seq.fold iter 0

    static member private allEqual (x : seq<Node>) (y : seq<Node>) =
        use ex = x.GetEnumerator()
        use ey = y.GetEnumerator()
        let rec loop () =
            match ex.MoveNext(), ey.MoveNext() with
            | false, false -> true
            | false, true
            | true, false -> false
            | true, true ->
                if ex.Current = ey.Current then
                    loop ()
                else
                    false
        loop ()

    static member private allCompare (x : seq<Node>) (y : seq<Node>) =
        use ex = x.GetEnumerator()
        use ey = y.GetEnumerator()
        let rec loop () =
            match ex.MoveNext(), ey.MoveNext() with
            | false, false -> 0
            | false, true -> -1
            | true, false -> 1
            | true, true ->
                let cmp = compare ex.Current ey.Current
                if cmp = 0 then loop () else cmp
        loop ()

    static member private rank x =
        match x with
        | Nil -> 0
        | List(_, _) -> 1
        | Vector(_, _) -> 2
        | Map(_, _) -> 3
        | Symbol(_) -> 4
        | Keyword(_) -> 5
        | Number(_) -> 6
        | String(_) -> 7
        | Bool(_) -> 8
        | BuiltinReference(_)
        | Func(_, _, _, _, _)
        | Macro(_, _, _, _, _) -> 9
        | DeviceState(_)
        | DeviceReference(_) -> 10
        | Atom(_, _) -> 11

    static member private equals x y =
        match x, y with
        | Nil, Nil -> true
        | List(_, a), List(_, b) -> a = b
        | List(_, a), Vector(_, b) -> Node.allEqual a b
        | Vector(_, a), List(_, b) -> Node.allEqual a b
        | Vector(_, a), Vector(_, b) -> Node.allEqual a b
        | Map(_, a), Map(_, b) -> a = b
        | Symbol(a), Symbol(b) -> a = b
        | Keyword(a), Keyword(b) -> a = b
        | Number(a), Number(b) -> a = b
        | String(a), String(b) -> a = b
        | Bool(a), Bool(b) -> a = b
        | BuiltinReference(a), BuiltinReference(b) -> a = b
        | (Func(_, a, _, _, _) | Macro(_, a, _, _, _)),
            (Func(_, b, _, _, _) | Macro(_, b, _, _, _)) ->
            a = b
        | DeviceState(a, b), DeviceState(c, d) -> a = c && b = d
        | DeviceReference(a, _), DeviceReference(b, _) -> a = b
        | Atom(a, _), Atom(b, _) -> a = b
        | _, _ -> false

    static member private compare x y =
        match x, y with
        | Nil, Nil -> 0
        | List(_, a), List(_, b) -> compare a b
        | List(_, a), Vector(_, b) -> Node.allCompare a b
        | Vector(_, a), List(_, b) -> Node.allCompare a b
        | Vector(_, a), Vector(_, b) -> Node.allCompare a b
        | Map(_, a), Map(_, b) -> compare a b
        | Symbol(a), Symbol(b) -> compare a b
        | Keyword(a), Keyword(b) -> compare a b
        | Number(a), Number(b) -> compare a b
        | String(a), String(b) -> compare a b
        | Bool(a), Bool(b) -> compare a b
        | DeviceState(a, m), DeviceState(b, n)-> compare (hash (a, m)) (hash (b, n))
        | DeviceReference(a, _), DeviceReference(b, _) -> compare a b
        | (Func(_, a, _, _, _) | Macro(_, a, _, _, _)),
            (Func(_, b, _, _, _) | Macro(_, b, _, _, _)) ->
            compare a b
        | Atom(a, _), Atom(b, _) -> compare a b
        | a, b -> compare (Node.rank a) (Node.rank b)

    override x.ToString() =
        "Node"

    override x.Equals yobj =
        match yobj with
        | :? Node as y -> Node.equals x y
        | _ -> false

    override x.GetHashCode() =
        match x with
        | Nil -> 0
        | List(_, lst) -> hash lst
        | Vector(_, vec) -> Node.hashSeq vec
        | Map(_, map) -> hash map
        | Symbol(sym) -> hash sym
        | Keyword(key) -> hash key
        | Number(num) -> hash num
        | String(str) -> hash str
        | Bool(b) -> hash b
        | BuiltinReference(name) -> hash name
        | Func(_, tag, _, _, _) | Macro(_, tag, _, _, _) ->
            hash tag
        | DeviceState(d, m) -> hash (d, m)
        | DeviceReference(d, _) -> hash d
        | Atom(tag, _) -> hash tag

    interface System.IComparable with
        member x.CompareTo yobj =
            match yobj with
            | :? Node as y -> Node.compare x y
            | _ -> invalidArg "yobj" "Cannot compare values of different types."

and AttributeState = AttributeState of value: Node * timestamp: NumberType
and Env = System.Collections.Generic.Dictionary<string, Node>
and EnvChain = Env list
and Metadata = Node

let toNativeOption (o: Optional.Option<'a>): 'a option = o.Match((fun t -> Option.Some t), (fun () -> Option.None))