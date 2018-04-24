module MyRE.Lisp.SmartApp

open MyRE.Lisp
open MyRE.Lisp.Types
open MyRE.Lisp.Node
open MyRE.Lisp.Core

open MyRE.SmartApp.Api.Client.Models
open System.Threading.Tasks
open MyRE.SmartApp.Api.Client

let private handleSmartThingsResponse (res: ApiResponse<'a>) =
    match toNativeOption res.Error with
    | Option.Some x -> Error.smartThingsFailure x.Message |> raise
    | Option.None -> res.Data

let private request (makeReq: unit -> 'a ApiResponse Task) =
    async { return! makeReq() |> Async.AwaitTask } |> Async.RunSynchronously |> handleSmartThingsResponse

type SmartAppClient(client: IMyreSmartAppApiClient) =
    let retrieveAllDeviceInfo () = 
        request (fun () -> client.ListDevicesAsync())

    let retrieveDeviceState = function
        | DeviceReference(devId) ->  
            request (fun () -> client.GetDeviceStatusAsync(devId))
        | _ -> Error.argMismatch() |> raise

    member this.getAllDeviceReferences eval env = function
        | [] -> 
            retrieveAllDeviceInfo()
                |> Seq.map (fun (di) -> DeviceReference(di.DeviceId)) |> List.ofSeq
                                |> makeList
        | _ -> raise <| Error.wrongArity ()

    member this.getDeviceReference eval env = function
        | [form;] -> 
            match eval env form with 
            | Node.String(devIdentifier) ->
                retrieveAllDeviceInfo()
                    |> Seq.filter (fun di -> di.DeviceId = devIdentifier || di.Label = devIdentifier)
                    |> Seq.map (fun di -> di.DeviceId)
                    |> Seq.head
                    |> DeviceReference
            | _ -> raise <| Error.argMismatch ()
        | _ -> raise <| Error.wrongArity ()

    member this.getDeviceState eval env = function 
        | [form;] ->
            match eval env form with
                | DeviceReference(d) as dref -> dref
                | String(_) as dIdentifier ->
                    this.getDeviceReference eval env [dIdentifier]
                | _ -> Error.argMismatch() |> raise
            |> retrieveDeviceState
            |> Node.DeviceState
        | _ -> Error.wrongArity() |> raise