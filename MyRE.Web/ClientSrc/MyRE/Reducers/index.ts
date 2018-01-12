import { Store } from "MyRE/Models/Store";
import { reduceNav } from "MyRE/Reducers/Nav";
import { none } from "ts-option";
import { reduceAuth } from "MyRE/Reducers/Auth";
import { List } from "immutable";
import { AppAction } from "MyRE/Actions";
import { reduceInstanceState } from "MyRE/Reducers/InstanceState";

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

    instanceState: {
        instances: none,
    }
}

export const reduce = (state: Store.All, action: any): Store.All => ({
    auth: reduceAuth(state.auth, action),
    nav: reduceNav(state.nav, action),
    instanceState: reduceInstanceState(state.instanceState, action),
})