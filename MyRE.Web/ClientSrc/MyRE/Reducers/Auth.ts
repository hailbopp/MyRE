import { Store } from "MyRE/Models/Store";
import { AppAction } from "MyRE/Actions";
import * as AuthActions from 'MyRE/Actions/Auth';
import { some, none, Some } from "ts-option";

const baseAuthFormState: Store.EmailPasswordForm = {
    emailValue: '',
    passwordValue: ''
}

export const reduceAuth = (state: Store.Auth, action: AppAction): Store.Auth => {
    var newState = Object.assign({}, state);

    switch (action.type) {
        case 'API_RESPONSE':
            if (action.requestType === 'API_REQUEST_CURRENT_USER') {
                if (action.response.result === "success") {
                    newState.currentUser = some(action.response.data);
                    newState.isLoggedIn = some(true);
                } else {
                    newState.isLoggedIn = some(false);
                }
            } else if (action.requestType === 'API_ATTEMPT_REGISTER') {
                if (action.response.result === "success") {
                    newState.registrationMessage = some({
                        level: 'success',
                        message: "Account created. Please log in.",
                    });
                    newState.registrationForm = Object.assign({}, baseAuthFormState);
                    newState.loginForm = Object.assign({}, baseAuthFormState);
                } else {
                    newState.registrationMessage = some({
                        level: "danger",
                        message: action.response.message.getOrElse("There was a problem completing your registration."),
                    });
                }
            } else if (action.requestType === 'API_ATTEMPT_LOGIN') {
                if (action.response.result === "success") {
                    newState.isLoggedIn = some(true);
                    newState.registrationForm = Object.assign({}, baseAuthFormState);
                    newState.loginForm = Object.assign({}, baseAuthFormState);
                } else {
                    newState.loginMessage = some({
                        level: "danger",
                        message: action.response.message.getOrElse("There was a problem logging in."),
                    });
                    return newState;
                }
            } else if (action.requestType === 'API_LOGOUT') {
                if (action.response.result === 'success') {
                    newState.currentUser = none;
                    newState.isLoggedIn = some(false);
                }
            }
            return newState;

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

        default:
            return newState;
    }
}