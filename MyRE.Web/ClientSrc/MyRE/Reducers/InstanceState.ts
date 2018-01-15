import { Store } from "MyRE/Models/Store";
import { AppAction } from "MyRE/Actions";
import { some, none } from "ts-option";

export const reduceInstanceState = (state: Store.InstanceState, action: AppAction): Store.InstanceState => {
    switch (action.type) {
        case 'API_RECEIVED_USER_INSTANCE_LIST':
            return {
                instances: some(action.instances),
                retrievingInstances: false,
            };
        case 'API_REQUEST_USER_INSTANCE_LIST':
            return {
                instances: state.instances,
                retrievingInstances: true,
            };

        case 'API_SUCCESSFUL_LOGIN':
        case 'API_SUCCESSFUL_LOGOUT':
            return {
                instances: none,
                retrievingInstances: false,
            };

        default:
            return state;
    }
}