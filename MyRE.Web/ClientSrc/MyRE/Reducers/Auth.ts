import { Store } from "MyRE/Models/Store";
import { AppAction } from "MyRE/Actions";
import { some, none, Some } from "ts-option";

const baseAuthFormState: Store.EmailPasswordForm = {
    emailValue: '',
    passwordValue: ''
}

export const reduceAuth = (state: Store.Auth, action: AppAction): Store.Auth => {
    var newState = Object.assign({}, state);

    switch (action.type) {
        case 'LOGIN_FORM_UPDATE':
            newState.loginForm = action.values;
            return newState;

        case 'REGISTRATION_FORM_UPDATE':
            newState.registrationForm = action.values;
            return newState;

        case 'CLEAR_AUTH_STATUS_MESSAGES':
            newState.loginMessage = none;
            newState.registrationMessage = none;
            return newState;

        case 'API_RECEIVED_CURRENT_USER':
            newState.currentUser = some(action.user);
            newState.isLoggedIn = some(true);
            return newState;

        case 'API_FAILED_TO_GET_CURRENT_USER':
            newState.isLoggedIn = some(false);
            return newState;

        case 'API_FAILED_REGISTRATION':
            newState.registrationMessage = some({
                level: "danger",
                message: action.message.get,
            });
            return newState;

        case 'API_SUCCESSFUL_REGISTRATION':
            newState.registrationMessage = some({
                level: 'success',
                message: "Account created. Please log in.",
            });
            newState.registrationForm = Object.assign({}, baseAuthFormState);
            newState.loginForm = Object.assign({}, baseAuthFormState);
            return newState;

        case 'API_SUCCESSFUL_LOGIN':
            newState.isLoggedIn = some(true);
            newState.registrationForm = Object.assign({}, baseAuthFormState);
            newState.loginForm = Object.assign({}, baseAuthFormState);
            return newState;

        case 'API_FAILED_LOGIN':
            newState.loginMessage = some({
                level: "danger",
                message: action.message,
            });
            return newState;

        case 'API_SUCCESSFUL_LOGOUT':
            newState.currentUser = none;
            newState.isLoggedIn = some(false);
            return newState;

        default:
            return newState;
    }
}