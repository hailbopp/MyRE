import { Store } from "MyRE/Models/Store";
import { DeviceInfo } from "MyRE/Api/Models";
import { List } from "immutable";

export const filterDevices = (instanceId: string, instanceState: Store.InstanceState): List<DeviceInfo> => {
    return instanceState.instances
        .map(il => il.find(inst => !!inst && inst.instanceId === instanceId)
            .devices
            .getOrElse(List([])))
        .getOrElse(List([]));
}