import { List } from "immutable";
import { Instance, DeviceInfo } from "MyRE/Api/Models";
import { ApiResponseAction } from "MyRE/Actions";

export type UserInstanceListRequestApiAction = {
    type: 'API_REQUEST_USER_INSTANCE_LIST';
    asyncActionType: 'API_REQUEST';
    userId: string;
}

export type UserInstanceListResponseApiAction = ApiResponseAction<UserInstanceListRequestApiAction, List<Instance>>;

export const listUserInstances = (userId: string): UserInstanceListRequestApiAction => ({
    type: 'API_REQUEST_USER_INSTANCE_LIST',
    asyncActionType: 'API_REQUEST',
    userId,
});

export type InstanceDevicesRequestApiAction = {
    type: 'API_REQUEST_INSTANCE_DEVICES_LIST';
    asyncActionType: 'API_REQUEST';
    instanceId: string;
}
export type InstanceDevicesResponseApiAction = ApiResponseAction<InstanceDevicesRequestApiAction, List<DeviceInfo>>;
export const listInstanceDevices = (instanceId: string): InstanceDevicesRequestApiAction => ({
    type: 'API_REQUEST_INSTANCE_DEVICES_LIST',
    asyncActionType: 'API_REQUEST',
    instanceId
});

export type InstanceApiRequestAction =
    | UserInstanceListRequestApiAction
    | InstanceDevicesRequestApiAction;

export type InstanceApiResponseAction =
    | UserInstanceListResponseApiAction
    | InstanceDevicesResponseApiAction;

export type InstanceApiAction =
    | InstanceApiRequestAction
    | InstanceApiResponseAction;

export type InstanceAction =
    | InstanceApiAction;

