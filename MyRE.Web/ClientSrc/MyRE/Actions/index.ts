import * as Auth from 'MyRE/Actions/Auth';
import * as Nav from 'MyRE/Actions/Nav';

type UndefinedAction = { type: '' };

export type AppAction =
    | Auth.AttemptAccountRegistrationApiAction
    | Auth.SuccessfulRegistrationApiAction
    | Auth.FailedRegistrationApiAction

    | Auth.AttemptLoginApiAction
    | Auth.SuccessfulLoginApiAction
    | Auth.FailedLoginApiAction
    | Auth.LogOutApiAction
    | Auth.SuccessfulLogoutApiAction

    | Auth.RequestCurrentUserApiAction
    | Auth.ReceivedCurrentUserApiAction
    | Auth.FailedGetCurrentUserApiAction

    | Auth.ClearAuthScreenStatusMessagesAction

    | Nav.OpenNavPaneAction
    | Nav.CloseNavPaneAction

    | UndefinedAction