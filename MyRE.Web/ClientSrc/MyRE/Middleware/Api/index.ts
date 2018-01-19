import * as Redux from "redux";
import { AppAction, ApiResponseAction, ApiRequestAction } from "MyRE/Actions";
import { ApiClient } from "MyRE/Middleware/Api/Client";
import { ApiError, ApiResult } from "MyRE/Api/Models/Results";
import { Store } from "MyRE/Models/Store";
import { Middleware, MiddlewareAPI, Dispatch } from "redux";
import { retrieveCurrentUser, clearAuthMessages, attemptLogin, RegisterRequestApiAction, AttemptLoginRequestApiAction, AttemptLoginResponseApiAction, LogOutRequestApiAction, GetCurrentUserRequestApiAction } from "MyRE/Actions/Auth";
import { some } from "ts-option";
import { List } from "immutable";
import { Instance, ProjectListing } from "MyRE/Api/Models";
import { requestProjectList, ProjectListRequestApiAction, CreateNewProjectRequestApiAction, DeleteProjectRequestApiAction } from "MyRE/Actions/Projects";
import { UserInstanceListRequestApiAction } from "MyRE/Actions/Instances";

function isAction(a: AppAction): a is AppAction {
    return (a as AppAction).type !== undefined;
}

function isApiRequestAction(a: AppAction): a is ApiRequestAction {
    let isReq = (a as ApiRequestAction).type !== undefined;
    let isApp = isAction(a);

    return isReq && isApp;
}

export interface ExtendedMiddleware<StateType> extends Middleware {
    <S extends StateType>(api: MiddlewareAPI<S>): (next: Dispatch<S>) => Dispatch<S>;
}

const apiResponse = <TRequest extends ApiRequestAction, TResponse>(action: TRequest, response: ApiResult<TResponse>): ApiResponseAction<TRequest, TResponse> => ({
    type: 'API_RESPONSE' as 'API_RESPONSE',
    action: action.type,
    response: response,
})

export const ApiServiceMiddleware: ExtendedMiddleware<Store.All> = <S extends Store.All>(api: MiddlewareAPI<S>) => (next: Dispatch<S>) => <A extends AppAction>(a: A): A => {
    if (isAction(a)) {
        let appAction = a as AppAction;

        next(appAction);

        if (isApiRequestAction(a)) {

            let action = appAction;

            const dispatch = (aa: AppAction) =>
                Promise.resolve()
                    .then(_ => api.dispatch(aa));

            switch (action.type) {
                // If we receive an action to send an API request, do it.
                case "API_ATTEMPT_LOGIN":
                    const innerAction = action as AttemptLoginRequestApiAction;
                    dispatch(clearAuthMessages());
                        ApiClient.logIn(action.credentials.email, action.credentials.password)
                            .then((result) => 
                                dispatch(apiResponse(innerAction, result)).then(_ => {
                                    if (result.result == 'success')
                                        return dispatch(retrieveCurrentUser());                                
                                    return _;
                                })
                            );
                    break;
                case "API_LOGOUT":
                    const logoutAction = action as LogOutRequestApiAction;
                    ApiClient.logOut()
                        .then(r => dispatch(apiResponse(logoutAction, r)));
                    break;
                case "API_ATTEMPT_REGISTER":
                    const registerAction = action as RegisterRequestApiAction;
                    dispatch(clearAuthMessages());
                    ApiClient.register(action.credentials.email, action.credentials.password)
                        .then((r) =>
                            dispatch(apiResponse(registerAction, r))
                                .then(_ => {
                                    if (r.result == 'success') {
                                        dispatch(attemptLogin((<RegisterRequestApiAction>action).credentials));
                                    }
                                }));
                    break;
                case "API_REQUEST_CURRENT_USER":
                    const getCurrentUserAction = action as GetCurrentUserRequestApiAction;
                    ApiClient.getCurrentUser()
                        .then((result) => dispatch(apiResponse(getCurrentUserAction, result)));
                    break;
                case "API_REQUEST_USER_INSTANCE_LIST":
                    ApiClient.listUserInstances(action.userId)
                        .then((result) => dispatch(apiResponse(action as UserInstanceListRequestApiAction, result)));
                    break;
                case 'API_REQUEST_PROJECT_LIST':
                    ApiClient.listProjects()
                        .then((result) => dispatch(apiResponse(action as ProjectListRequestApiAction, result)));
                    break;

                case 'API_CREATE_NEW_PROJECT':
                    ApiClient.createProject(action.newProject)
                        .then((result) => dispatch(apiResponse(action as CreateNewProjectRequestApiAction, result)).then(_ => { if (result.result === "success") dispatch(requestProjectList()); }));
                        
                    break;

                case 'API_DELETE_PROJECT':
                    ApiClient.deleteProject(action.projectId)
                        .then((result) => dispatch(apiResponse(action as DeleteProjectRequestApiAction, result)).then(_ => { if (result.result === "success") dispatch(requestProjectList()); }))
                    break;

                default:
                    return a;
            }

        }
    }
    return a;
};
