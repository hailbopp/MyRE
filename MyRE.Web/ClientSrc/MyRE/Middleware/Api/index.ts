import * as Redux from "redux";
import { AppAction } from "MyRE/Actions";
import { ApiClient } from "MyRE/Middleware/Api/Client";
import { ApiError } from "MyRE/Api/ApiClient";
import { Store } from "MyRE/Models/Store";
import { Middleware, MiddlewareAPI, Dispatch } from "redux";
import { retrieveCurrentUser, clearAuthMessages } from "MyRE/Actions/Auth";

function isAction(a: AppAction): a is AppAction {
    return (a as AppAction).type !== undefined;
}


export interface ExtendedMiddleware<StateType> extends Middleware {
    <S extends StateType>(api: MiddlewareAPI<S>): (next: Dispatch<S>) => Dispatch<S>;
}

export const ApiServiceMiddleware: ExtendedMiddleware<Store.All> = <S extends Store.All>(api: MiddlewareAPI<S>) => (next: Dispatch<S>) => <A extends AppAction>(a: A): A => {
    if (isAction(a)) {
        let action = a as AppAction;

        const dispatch = (aa: AppAction) =>
            Promise.resolve()
                .then(_ => api.dispatch(aa));        

        // Pass all actions through by default.
        next(action);

        switch (action.type) {
            // If we receive an action to send an API request, do it.
            case "API_ATTEMPT_LOGIN":
                dispatch(clearAuthMessages());
                ApiClient.logIn(action.credentials.email, action.credentials.password)
                    .then((result) => {
                        if (result.result === 'error') {
                            dispatch({
                                type: 'API_FAILED_LOGIN'
                            });
                        } else {
                            dispatch({
                                type: 'API_SUCCESSFUL_LOGIN'
                            });
                            dispatch(retrieveCurrentUser());
                        }
                    });
                break;
            case "API_LOGOUT":
                ApiClient.logOut()
                    .then(r => {
                        if (r.result === "success") {
                            dispatch({
                                type: 'API_SUCCESSFUL_LOGOUT',
                            });
                        }
                    });
                break;
            case "API_ATTEMPT_REGISTER":
                dispatch(clearAuthMessages());
                ApiClient.register(action.credentials.email, action.credentials.password)
                    .then((r) => {
                        if (r.result === "error") {
                            dispatch({
                                type: 'API_FAILED_REGISTRATION',
                                message: r.message
                            });
                        } else {
                            dispatch({
                                type: 'API_SUCCESSFUL_REGISTRATION',
                            });
                        }
                    })
                break;
            case "API_REQUEST_CURRENT_USER":
                ApiClient.getCurrentUser()
                    .then((result) => {
                        if (result.result === 'error') {
                            dispatch({ type: 'API_FAILED_TO_GET_CURRENT_USER' });
                        } else {
                            dispatch({ type: 'API_RECEIVED_CURRENT_USER', user: result.data })
                        }
                    })
                break;
            default:
                return a;
        }
    }
    return a;
};
