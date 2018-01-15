import { Store } from "MyRE/Models/Store";
import { reduceNav } from "MyRE/Reducers/Nav";
import { none } from "ts-option";
import { reduceAuth } from "MyRE/Reducers/Auth";
import { List } from "immutable";
import { AppAction } from "MyRE/Actions";
import { reduceInstanceState } from "MyRE/Reducers/InstanceState";
import { reduceProjects } from "MyRE/Reducers/Projects";

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
        retrievingInstances: false,
    },

    projects: {
        projects: none,
        retrievingProjects: false,

        createProjectModalOpen: false,
        newProject: {
            Name: '',
            Description: '',
            InstanceId: -1,
        },
        newProjectSubmitting: false,
        createProjectMessage: none,
    }
}

export const reduce = (state: Store.All, action: any): Store.All => ({
    auth: reduceAuth(state.auth, action),
    nav: reduceNav(state.nav, action),
    instanceState: reduceInstanceState(state.instanceState, action),
    projects: reduceProjects(state.projects, action),
})