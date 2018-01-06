import { Option } from 'ts-option';
import { List } from 'immutable';
import { User } from "MyRE/Api/Models";
import { AppAction } from 'MyRE/Actions';


export namespace Store {
    export interface AlertMessage {
        level: string;
        message: string;
    }

    export interface Nav {
        navPaneOpen: boolean;
    }

    export interface Auth {
        // isLoggedIn has some extra implications.
        // If isLoggedIn is none, then we haven't tried to check for a logged in user yet.
        // If it's some(true), then currentUser should also be populated.
        // If it's some(false), then we should be showing the login/register page.
        isLoggedIn: Option<boolean>; 
        currentUser: Option<User>;

        loginMessage: Option<AlertMessage>;
        registrationMessage: Option<AlertMessage>;
    }
    
    export interface Script {
        name: string;
    }

    export interface All {
        auth: Auth;

        nav: Nav;
    }
}