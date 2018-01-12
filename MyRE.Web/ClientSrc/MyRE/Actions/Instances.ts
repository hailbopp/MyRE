import { List } from "immutable";
import { Instance } from "MyRE/Api/Models";



export type RequestUserInstanceListApiAction = {
    type: 'API_REQUEST_USER_INSTANCE_LIST';
    userId: string;
}

export type ReceivedUserInstanceListApiAction = {
    type: 'API_RECEIVED_USER_INSTANCE_LIST';
    instances: List<Instance>;
}

export const listUserInstances = (userId: string): RequestUserInstanceListApiAction => ({
    type: 'API_REQUEST_USER_INSTANCE_LIST',
    userId,
});