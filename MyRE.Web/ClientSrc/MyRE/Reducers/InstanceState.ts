import { Store } from "MyRE/Models/Store";
import { AppAction } from "MyRE/Actions";
import { some, none } from "ts-option";
import { UserInstanceListResponseApiAction } from "MyRE/Actions/Instances";
import { AttemptLoginResponseApiAction, LogOutResponseApiAction } from "MyRE/Actions/Auth";

export const reduceInstanceState = (state: Store.InstanceState, action: AppAction): Store.InstanceState => {
    switch (action.type) {
        case 'API_RESPONSE':
            if (action.requestType === 'API_REQUEST_USER_INSTANCE_LIST') {
                if (action.response.result === "success") {
                    return {
                        instances: some(action.response.data),
                    };
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