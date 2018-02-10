import { Store } from "MyRE/Models/Store";
import { reduceNav } from "MyRE/Reducers/Nav";
import { none } from "ts-option";
import { reduceAuth } from "MyRE/Reducers/Auth";
import { List } from "immutable";
import { AppAction } from "MyRE/Actions";
import { reduceInstanceState } from "MyRE/Reducers/InstanceState";
import { reduceProjects } from "MyRE/Reducers/Projects";
import { reduceAsyncActions } from "MyRE/Reducers/AsyncActions";

export const initialState: Store.All = {
    auth: {
        currentUser: none,
        isLoggedIn: none,

        loginMessage: none,
        registrationMessage: none,

        loginForm: {
            emailValue: '',
            passwordValue: ''
        },
        registrationForm: {
            emailValue: '',
            passwordValue: ''
        }
    },

    nav: {
        navPaneOpen: false,
    },

    instanceState: {
        instances: none,
    },

    projects: {
        projects: none,
        activeProject: none,

        createProjectModalOpen: false,
        newProject: {
            Name: '',
            Description: '',
            InstanceId: '',
        },
        createProjectMessage: none,
    },

    asyncActions: {
        currentAsyncActions: List([])
    },
}

export const reduce = (state: Store.All, action: any): Store.All => ({
    auth: reduceAuth(state.auth, action),
    nav: reduceNav(state.nav, action),
    instanceState: reduceInstanceState(state.instanceState, action),
    projects: reduceProjects(state.projects, action),
    asyncActions: reduceAsyncActions(state.asyncActions, action),
})