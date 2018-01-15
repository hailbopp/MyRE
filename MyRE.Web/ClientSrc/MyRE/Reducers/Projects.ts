import { Store } from "MyRE/Models/Store";
import { AppAction } from "MyRE/Actions";
import { some, none } from "ts-option";
import { List } from "immutable";

export const reduceProjects = (state: Store.Projects, action: AppAction): Store.Projects => {
    let newState = Object.assign({}, state);

    switch (action.type) {
        case 'API_REQUEST_PROJECT_LIST':
            newState.retrievingProjects = true
            return newState;

        case 'API_RECEIVED_PROJECTS':
            newState.projects = some(action.projects);
            newState.retrievingProjects = false;
            return newState;

        case 'API_FAILED_LOGIN':
            newState.projects = some(List([]));
            newState.retrievingProjects = false;
            return newState;

        case 'API_SUCCESSFUL_LOGIN':
        case 'API_SUCCESSFUL_LOGOUT':
            newState.projects = none;
            newState.retrievingProjects = false;
            return newState;

        case 'UI_TOGGLE_CREATE_PROJECT_DIALOG':
            if (state.createProjectModalOpen) {
                // Close the modal and clear out state data.
                newState.createProjectModalOpen = false;
                newState.newProject = { Name: '', Description: '', InstanceId: '' };
            } else {
                newState.createProjectModalOpen = true;
            }
            return newState;

        case 'UI_CHANGE_NEW_PROJECT_DATA':
            newState.newProject = action.value;
            return newState;

        case 'API_CREATE_NEW_PROJECT':
            newState.newProjectSubmitting = true;
            newState.createProjectMessage = none;
            return newState;

        case 'API_SUCCESSFUL_CREATE_PROJECT':
            newState.newProjectSubmitting = false;
            newState.createProjectModalOpen = false;
            newState.createProjectMessage = none;
            newState.newProject = { Name: '', Description: '', InstanceId: '' };
            return newState;

        case 'API_FAILED_CREATE_PROJECT':
            newState.newProjectSubmitting = false;
            newState.createProjectMessage = some<Store.AlertMessage>({
                level: 'danger',
                message: action.error.message.getOrElse(action.error.status.toString()),
            });
            return newState;

        default:
            return state;
    }
}