import deepEqual = require('deep-equal');
import { Store } from "MyRE/Models/Store";
import { AppAction, ApiRequestAction } from "MyRE/Actions";

const isApiRequestAction = (a: AppAction): a is ApiRequestAction => {
    let cast = a as ApiRequestAction;
    return cast.asyncActionType !== undefined && cast.asyncActionType === 'API_REQUEST';
}

export const reduceAsyncActions = (state: Store.AsyncActions, action: AppAction): Store.AsyncActions => {
    if (isApiRequestAction(action)) {
        return {
            currentAsyncActions: state.currentAsyncActions.insert(0, action)
        };
    } else if (action.type === 'API_RESPONSE') {
        return {
            currentAsyncActions: state.currentAsyncActions.filterNot(req => deepEqual(req, action.requestAction, { strict: true })).toList(),
        }
    }

    return state;
}