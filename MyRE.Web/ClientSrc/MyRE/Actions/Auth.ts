import { User } from "MyRE/Api/Models";
import { Option, some } from "ts-option";

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
