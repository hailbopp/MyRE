import { Store } from "MyRE/Models/Store";
import { AppAction } from "MyRE/Actions";
import { some } from "ts-option";



export const reduceInstanceState = (state: Store.InstanceState, action: AppAction): Store.InstanceState => {
    switch (action.type) {
        case 'API_RECEIVED_USER_INSTANCE_LIST':
            return {
                instances: some(action.instances)
            }
        default:
            return state;
    }
}