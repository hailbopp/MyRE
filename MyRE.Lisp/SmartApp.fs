module MyRE.Lisp.SmartApp

open MyRE.Lisp
open MyRE.Lisp.Types
open MyRE.Lisp.Node
open MyRE.Lisp.Core

open MyRE.SmartApp.Api.Client.Models
open System.Threading.Tasks
open MyRE.SmartApp.Api.Client
open System
open System.Collections.Concurrent
open MyRE.Lisp.Types

let inline (|>>|) a b = b, a

let private handleSmartThingsResponse (res: ApiResponse<'a>) =
    match toNativeOption res.Error with
    | Option.Some x -> Error.smartThingsFailure x.Message |> raise
    | Option.None -> res.Data

let private request (makeReq: unit -> 'a ApiResponse Task) =
    async { return! makeReq() |> Async.AwaitTask } |> Async.RunSynchronously |> handleSmartThingsResponse

type SmartAppClient(client: IMyreSmartAppApiClient) =
    let memoize cacheTimeSeconds (caller:string) (f: ('a -> 'b)) =
        let cacheTimes = ConcurrentDictionary<string,DateTime>()
        let cache = ConcurrentDictionary<'a, 'b>()    
        fun x ->
            match cacheTimes.TryGetValue caller with
            | true, time when time < DateTime.UtcNow.AddSeconds(-cacheTimeSeconds)
                -> cache.TryRemove(x) |> ignore
            | _ -> ()
            cache.GetOrAdd(x, fun x -> 
                cacheTimes.AddOrUpdate(caller, DateTime.UtcNow, fun _ _ ->DateTime.UtcNow)|> ignore
                f(x)
                )

    let memoizeAsync cacheTimeSeconds (caller:string) (f: ('a -> Async<'b>)) =
        let cacheTimes = ConcurrentDictionary<string,DateTime>()
        let cache = ConcurrentDictionary<'a, System.Threading.Tasks.Task<'b>>()    
        fun x ->
            match cacheTimes.TryGetValue caller with
            | true, time when time < DateTime.UtcNow.AddSeconds(-cacheTimeSeconds)
                -> cache.TryRemove(x) |> ignore
            | _ -> ()
            cache.GetOrAdd(x, fun x -> 
                cacheTimes.AddOrUpdate(caller, DateTime.UtcNow, fun _ _ ->DateTime.UtcNow)|> ignore
                f(x) |> Async.StartAsTask
                ) |> Async.AwaitTask

    let retrieveAllDeviceInfo = 
        memoize 30. "client" (fun () -> request (fun () -> client.ListDevicesAsync()))

    let getDeviceInfo devId = retrieveAllDeviceInfo() |> Seq.find (fun d -> d.DeviceId = devId)

    let callCommandAsync devId cmdName args =
        let req = new ExecuteDeviceCommandRequest(Parameters=args)
        client.ExecuteDeviceCommandAsync(devId, cmdName, req).Start()

    let rec convertNodeToSmartThingsArg (node: Node) =
        match node with
        | Number n -> n :> obj
        | String s -> s :> obj
        | List(_, l) -> List.map convertNodeToSmartThingsArg l :> obj
        | Vector(_, l) -> l.Array |> Array.map convertNodeToSmartThingsArg :> obj
        | Bool b -> b :> obj
        | _ -> null
    
    let createNodeFromAttributeValue (attr: AttributeInfo) (attributeValue: string) =        
        match attr.Type with
        | "NUMBER" 
        | "DATE" ->
            match Decimal.TryParse attributeValue with
            | true, d -> Node.Number d
            | _ -> Node.String attributeValue
        | "ENUM" 
        | "STRING" ->
            Node.String attributeValue
        | "VECTOR3" ->
            match attributeValue.Split [| ',' |] with
            | [| a; b; c |] -> 
                (Node.NIL, [a; b; c;]
                                |> List.map (fun n ->
                                    match Decimal.TryParse n with
                                    | true, d -> Node.Number d
                                    | _ -> Node.String attributeValue)
                                |> Array.ofList
                                |> ArraySegment
                ) |> Node.Vector

            | _ -> Node.String attributeValue

        | _ -> Node.String attributeValue


    let retrieveDeviceState = function
        | DeviceReference(devId, _) ->  
            let result = request (fun () -> client.GetDeviceStatusAsync(devId))
            let devInfo = getDeviceInfo devId
            
            result.AttributeStates
            |> Seq.map (fun attr -> 
                    attr, devInfo.Attributes |> Seq.find (fun ainfo -> ainfo.Name = attr.Name)
                )
            |> Seq.map (fun (attr, aInfo) -> 
                Node.String attr.Name, Node.makeMap ([
                                                        Node.String "value", createNodeFromAttributeValue aInfo attr.Value; 
                                                        Node.String "asOf", Node.Number <| (attr.Timestamp.ToUnixTimeMilliseconds() |> string |> NumberType.Parse);
                                                     ] |> Map.ofList))
            |> Map.ofSeq
        | _ -> Error.argMismatch() |> raise

    member this.getAllDeviceReferences eval env = function
        | [] -> 
            retrieveAllDeviceInfo()
                |> Seq.map (fun (di) -> DeviceReference(di.DeviceId, di.Label)) |> List.ofSeq
                                |> makeList
        | _ -> raise <| Error.wrongArity ()

    member this.getDeviceReference eval env = function
        | [form;] -> 
            match eval env form with 
            | Node.String(devIdentifier) ->
                retrieveAllDeviceInfo()
                    |> Seq.filter (fun di -> di.DeviceId = devIdentifier || di.Label = devIdentifier)
                    |> Seq.map (fun di -> di.DeviceId, di.Label)
                    |> Seq.head
                    |> DeviceReference
            | _ -> raise <| Error.argMismatch ()
        | _ -> raise <| Error.wrongArity ()

    member this.getDeviceState eval env args =
        match args with
        | [form;] ->
            let (devId, devRef) = 
                match eval env form with
                    | DeviceReference(d, l) as devReference -> d, devReference
                    | String(_) as dIdentifier ->
                        match this.getDeviceReference eval env [dIdentifier] with
                        | DeviceReference(d, l) as devReference -> d, devReference
                        | _ -> Error.argMismatch() |> raise     
                    | _ -> Error.argMismatch() |> raise

            devRef
            |> retrieveDeviceState
            |> Node.makeMap
            |>>| devId
            |> Node.DeviceState
        | _ -> Error.wrongArity() |> raise

    member this.getAttributeState eval env args =
        match args |> List.map (eval env) with
        | [DeviceState(dId, attrMapNode); String(_) as attrName;] ->
            match attrMapNode with
            | Map(_, attrMap) -> Map.find attrName attrMap
            | _ -> Error.argMismatch() |> raise
        | _ -> Error.argMismatch() |> raise
        
    member this.callDeviceCommand eval env args =
        match List.map (eval env) args with
        | [DeviceReference(dId, _), String(cmdName), Vector(_, paramsArg)]
        | [DeviceState(dId, _), String(cmdName), Vector(_, paramsArg)] ->
            let pList = paramsArg.Array |> Array.map convertNodeToSmartThingsArg
            callCommandAsync dId cmdName pList
        | _ -> Error.argMismatch() |> raise
