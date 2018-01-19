import * as Auth from 'MyRE/Actions/Auth';
import * as Nav from 'MyRE/Actions/Nav';
import * as Instances from 'MyRE/Actions/Instances';
import * as Projects from 'MyRE/Actions/Projects';
import { ApiResult } from 'MyRE/Api/Models/Results';

type UndefinedAction = { type: '' };

export type ApiRequestAction =
    | Auth.AuthApiRequestAction
    | Projects.ProjectApiRequestAction
    | Instances.InstanceApiRequestAction;    

export interface ApiResponseAction<TRequestAction extends ApiRequestAction, TResponse> {
    type: 'API_RESPONSE';
    action: TRequestAction['type'];
    response: ApiResult<TResponse>;
}

export type AppAction =
    | Auth.AuthAction
    | Nav.NavAction

    | Instances.InstanceAction

    | Projects.ProjectAction

    | UndefinedAction;
