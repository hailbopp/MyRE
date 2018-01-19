import { Store } from "MyRE/Models/Store";
import { AppAction } from "MyRE/Actions";
import { some, none } from "ts-option";
import { List } from "immutable";
import * as ProjectActions from 'MyRE/Actions/Projects';

export const reduceProjects = (state: Store.Projects, action: AppAction): Store.Projects => {
    let newState = Object.assign({}, state);

    switch (action.type) {
        case 'API_RESPONSE':
            if (action.action === 'API_REQUEST_PROJECT_LIST') {
                if (action.response.result === 'success') {
                    newState.projects = some(List(action.response.data.map(p => ({
                        projectId: p.ProjectId,
                        name: p.Name,
                        description: p.Description,
                        instanceId: p.InstanceId,
                        routines: none,
                    }))));
                    newState.retrievingProjects = false;
                }
            } else if (action.action === 'API_CREATE_NEW_PROJECT') {
                if (action.response.result === "success") {
                    newState.newProjectSubmitting = false;
                    newState.createProjectModalOpen = false;
                    newState.createProjectMessage = none;
                    newState.newProject = { Name: '', Description: '', InstanceId: '' };
                } else {
                    newState.newProjectSubmitting = false;
                    newState.createProjectMessage = some<Store.AlertMessage>({
                        level: 'danger',
                        message: action.response.message.getOrElse(action.response.status.toString()),
                    });
                }
            } else if (action.action === 'API_LOGOUT') {
                if (action.response.result === "success") {
                    newState.projects = none;
                    newState.retrievingProjects = false;
                }
            } else if (action.action === 'API_ATTEMPT_LOGIN') {
                if (action.response.result === "success") {
                    newState.projects = none;
                    newState.retrievingProjects = false;
                } else {
                    newState.projects = some(List([]));
                    newState.retrievingProjects = false;
                }
            }
            return newState;

        case 'API_REQUEST_PROJECT_LIST':
            newState.retrievingProjects = true
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
            
        default:
            return state;
    }
}