import * as Auth from 'FreeCoRE/Actions/Auth';
import * as Nav from 'FreeCoRE/Actions/Nav';

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