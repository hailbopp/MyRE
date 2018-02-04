import { Store } from "MyRE/Models/Store";
import { AppAction } from "MyRE/Actions";
import { some, none } from "ts-option";
import { UserInstanceListResponseApiAction } from "MyRE/Actions/Instances";
import { AttemptLoginResponseApiAction, LogOutResponseApiAction } from "MyRE/Actions/Auth";
import { List } from "immutable";

export const reduceInstanceState = (state: Store.InstanceState, action: AppAction): Store.InstanceState => {
    let newState = Object.assign({}, state);

    switch (action.type) {
        case 'API_RESPONSE':
            if (action.requestType === 'API_REQUEST_USER_INSTANCE_LIST') {
                if (action.response.result === "success") {
                    return {
                        instances: some(action.response.data.map((i): Store.Instance => ({
                            instanceId: i.InstanceId,
                            accountId: i.AccountId,
                            name: i.Name,
                            devices: none,
                        })).toList()),
                    };
                }
            } else if (action.requestType === 'API_REQUEST_INSTANCE_DEVICES_LIST') {
                if (action.response.result === 'success') {
                    const devices = action.response.data;
                    newState.instances = state.instances.map(o => o.map((i): Store.Instance => {
                        if (action.requestAction.instanceId === i.instanceId) {
                            return {
                                accountId: i.accountId,
                                instanceId: i.instanceId,
                                name: i.name,
                                devices: some(devices),
                            }
                        }
                        return i;
                    }).toList());

                    return newState;
                }
            } else if (action.requestType === 'API_ATTEMPT_LOGIN' || action.requestType === 'API_LOGOUT') {
                if (action.response.result === "success") {
                    return {
                        instances: none,
                    };
                }
            }

        default:
            return state;
    }
}