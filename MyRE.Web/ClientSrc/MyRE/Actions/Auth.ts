import { User } from "MyRE/Api/Models";
import { Option, some } from "ts-option";
import { Store } from "MyRE/Models/Store";
import { ApiResponseAction } from "MyRE/Actions";
import { ApiError } from "MyRE/Api/Models/Results";

export interface Credentials {
    email: string;
    password: string;
}

// Clear messages
export type ClearAuthScreenStatusMessagesAction = {
    type: 'CLEAR_AUTH_STATUS_MESSAGES';
}

export const clearAuthMessages = (): ClearAuthScreenStatusMessagesAction => ({
    type: 'CLEAR_AUTH_STATUS_MESSAGES',
})

// Get current User
export type GetCurrentUserRequestApiAction = {
    type: 'API_REQUEST_CURRENT_USER';
    asyncActionType: 'API_REQUEST';
}

export type GetCurrentUserResponseApiAction = ApiResponseAction<GetCurrentUserRequestApiAction, User>;

export const retrieveCurrentUser = (): GetCurrentUserRequestApiAction => ({
    type: 'API_REQUEST_CURRENT_USER',
    asyncActionType: 'API_REQUEST',
});

// Login/Registration Form Values
export type UpdateLoginFormValues = {
    type: 'LOGIN_FORM_UPDATE';
    values: Store.EmailPasswordForm;
}

export type UpdateRegistrationFormValues = {
    type: 'REGISTRATION_FORM_UPDATE';
    values: Store.EmailPasswordForm;
}

export const updateLoginForm = (values: Store.EmailPasswordForm): UpdateLoginFormValues => ({
    type: 'LOGIN_FORM_UPDATE',
    values,
});

export const updateRegistrationForm = (values: Store.EmailPasswordForm): UpdateRegistrationFormValues => ({
    type: 'REGISTRATION_FORM_UPDATE',
    values,
});

// Log in
export type AttemptLoginRequestApiAction = {
    type: 'API_ATTEMPT_LOGIN';
    asyncActionType: 'API_REQUEST';
    credentials: Credentials;
}
export type AttemptLoginResponseApiAction = ApiResponseAction<AttemptLoginRequestApiAction, any>;

export const attemptLogin = (credentials: Credentials): AttemptLoginRequestApiAction => ({
    type: 'API_ATTEMPT_LOGIN',
    asyncActionType: 'API_REQUEST',
    credentials: credentials,
});

export const sendLoginError = (message: string): AttemptLoginResponseApiAction => ({
    type: 'API_RESPONSE',
    requestType: 'API_ATTEMPT_LOGIN',
    requestAction: {
        type: 'API_ATTEMPT_LOGIN', asyncActionType: 'API_REQUEST', credentials: { email: '', password: '' }
    },
    response: new ApiError(400, some(message)),
});

// Log out
export type LogOutRequestApiAction = {
    type: 'API_LOGOUT';
    asyncActionType: 'API_REQUEST';
}

export type LogOutResponseApiAction = ApiResponseAction<LogOutRequestApiAction, any>;

export const initiateLogout = (): LogOutRequestApiAction => ({
    type: 'API_LOGOUT',
    asyncActionType: 'API_REQUEST',
})

// Registration
export type RegisterRequestApiAction = {
    type: 'API_ATTEMPT_REGISTER';
    asyncActionType: 'API_REQUEST';
    credentials: Credentials;
}

export type RegisterResponseApiAction = ApiResponseAction<RegisterRequestApiAction, any>;

export const attemptRegistration = (credentials: Credentials): RegisterRequestApiAction => ({
    type: 'API_ATTEMPT_REGISTER',
    asyncActionType: 'API_REQUEST',
    credentials,
});

export type AuthApiRequestAction =
    | RegisterRequestApiAction
    | AttemptLoginRequestApiAction
    | LogOutRequestApiAction
    | GetCurrentUserRequestApiAction;

export type AuthApiResponseAction =
    | AttemptLoginResponseApiAction
    | RegisterResponseApiAction
    | LogOutResponseApiAction
    | GetCurrentUserResponseApiAction;

export type AuthApiAction =
    | AuthApiRequestAction
    | AuthApiResponseAction;

export type AuthUIAction =
    | UpdateLoginFormValues
    | UpdateRegistrationFormValues
    | ClearAuthScreenStatusMessagesAction;

export type AuthAction =
    | AuthApiAction
    | AuthUIAction;
