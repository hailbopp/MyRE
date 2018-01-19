import { List } from "immutable";
import { Instance } from "MyRE/Api/Models";
import { ApiResponseAction } from "MyRE/Actions";



export type UserInstanceListRequestApiAction = {
    type: 'API_REQUEST_USER_INSTANCE_LIST';
    userId: string;
}

export type UserInstanceListResponseApiAction = ApiResponseAction<UserInstanceListRequestApiAction, List<Instance>>;

export const listUserInstances = (userId: string): UserInstanceListRequestApiAction => ({
    type: 'API_REQUEST_USER_INSTANCE_LIST',
    userId,
});

export type InstanceApiRequestAction =
    | UserInstanceListRequestApiAction;

export type InstanceApiResponseAction =
    | UserInstanceListResponseApiAction;

export type InstanceApiAction =
    | InstanceApiRequestAction
    | InstanceApiResponseAction;

export type InstanceAction =
    | InstanceApiAction;

