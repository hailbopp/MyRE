import { Store } from "FreeCoRE/Models/Store";
import { reduceNav } from "FreeCoRE/Reducers/Nav";
import { none } from "ts-option";
import { reduceAuth } from "FreeCoRE/Reducers/Auth";
import { List } from "immutable";
import { AppAction } from "FreeCoRE/Actions";

export const initialState: Store.All = {
    auth: {
        currentUser: none,
        isLoggedIn: none,

        loginMessage: none,
        registrationMessage: none,
    },

    nav: {
        navPaneOpen: false,
    },
}

export const reduce = (state: Store.All, action: any): Store.All => ({
    auth: reduceAuth(state.auth, action),
    nav: reduceNav(state.nav, action),
})