import { User } from "MyRE/Api/Models";
import { Option, some } from "ts-option";
import { Store } from "MyRE/Models/Store";

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
export type RequestCurrentUserApiAction = {
    type: 'API_REQUEST_CURRENT_USER';
}

export type ReceivedCurrentUserApiAction = {
    type: 'API_RECEIVED_CURRENT_USER';
    user: User;
}

export type FailedGetCurrentUserApiAction = {
    type: 'API_FAILED_TO_GET_CURRENT_USER';
}

export const retrieveCurrentUser = (): RequestCurrentUserApiAction => ({
    type: 'API_REQUEST_CURRENT_USER',
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
export type AttemptLoginApiAction = {
    type: 'API_ATTEMPT_LOGIN';
    credentials: Credentials;
}

export type SuccessfulLoginApiAction = {
    type: 'API_SUCCESSFUL_LOGIN';
}

export type FailedLoginApiAction = {
    type: 'API_FAILED_LOGIN';
    message: string;
}

export const attemptLogin = (credentials: Credentials): AttemptLoginApiAction => ({
    type: 'API_ATTEMPT_LOGIN',
    credentials: credentials,
});

export const sendLoginError = (message: string): FailedLoginApiAction => ({
    type: 'API_FAILED_LOGIN',
    message: message
});

// Log out
export type LogOutApiAction = {
    type: 'API_LOGOUT';
}

export type SuccessfulLogoutApiAction = {
    type: 'API_SUCCESSFUL_LOGOUT';
}

export const initiateLogout = (): LogOutApiAction => ({
    type: 'API_LOGOUT',
});

// Registration
export type AttemptAccountRegistrationApiAction = {
    type: 'API_ATTEMPT_REGISTER';
    credentials: Credentials;
}

export type SuccessfulRegistrationApiAction = {
    type: 'API_SUCCESSFUL_REGISTRATION';
}

export type FailedRegistrationApiAction = {
    type: 'API_FAILED_REGISTRATION';
    message: Option<string>;
}

export const attemptRegistration = (credentials: Credentials): AttemptAccountRegistrationApiAction => ({
    type: 'API_ATTEMPT_REGISTER',
    credentials,
});

export type AuthAction =
    | UpdateLoginFormValues
    | UpdateRegistrationFormValues

    | AttemptAccountRegistrationApiAction
    | SuccessfulRegistrationApiAction
    | FailedRegistrationApiAction

    | AttemptLoginApiAction
    | SuccessfulLoginApiAction
    | FailedLoginApiAction
    | LogOutApiAction
    | SuccessfulLogoutApiAction

    | RequestCurrentUserApiAction
    | ReceivedCurrentUserApiAction
    | FailedGetCurrentUserApiAction

    | ClearAuthScreenStatusMessagesAction;
