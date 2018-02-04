import { Option } from 'ts-option';
import { List } from 'immutable';
import { User, ProjectListing, CreateProjectRequest, DeviceInfo } from "MyRE/Api/Models";
import { AppAction, ApiRequestAction } from 'MyRE/Actions';
import { Program } from 'MyRE/Utils/Models/DslModels';


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

    export interface Instance {
        instanceId: string;
        name: string;
        accountId: string;

        devices: Option<List<DeviceInfo>>
    }
    
    export interface InstanceState {
        // If instances is none, then we haven't yet attempted to grab instances from the API.
        instances: Option<List<Instance>>;
    }

    export interface Statement {
        statementId: string;
    }

    export interface BlockStatement {
        blockStatementId: string;
        position: number;
        statement: Option<Statement>;
    }

    export interface Project {
        projectId: string;
        name: string;
        description: string;
        instanceId: string;

        source: string;
        expressionTree: Program | SyntaxError;
    }

    export interface Projects {
        projects: Option<List<Project>>;

        createProjectModalOpen: boolean;
        newProject: CreateProjectRequest;
        createProjectMessage: Option<AlertMessage>;
    }    

    export interface AsyncActions {
        currentAsyncActions: List<ApiRequestAction>;
    }

    
    export interface All {
        auth: Auth;
        nav: Nav;
        instanceState: InstanceState;
        projects: Projects;

        asyncActions: AsyncActions;
    }
}