import { Option } from 'ts-option';
import { List } from 'immutable';
import { User, Instance, ProjectListing, CreateProjectRequest } from "MyRE/Api/Models";
import { AppAction } from 'MyRE/Actions';


export namespace Store {
    export interface AlertMessage {
        level: string;
        message: string;
    }

    export interface Nav {
        navPaneOpen: boolean;
    }

    export interface EmailPasswordForm {
        emailValue: string;
        passwordValue: string;
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

        loginForm: EmailPasswordForm;
        registrationForm: EmailPasswordForm;
    }
    
    export interface InstanceState {
        // If instances is none, then we haven't yet attempted to grab instances from the API.
        instances: Option<List<Instance>>;
        retrievingInstances: boolean;
    }

    export interface Projects {
        projects: Option<List<ProjectListing>>;
        retrievingProjects: boolean;

        createProjectModalOpen: boolean;
        newProject: CreateProjectRequest;
        newProjectSubmitting: boolean;
        createProjectMessage: Option<AlertMessage>;
    }

    export interface All {
        auth: Auth;
        nav: Nav;
        instanceState: InstanceState;
        projects: Projects;
    }
}