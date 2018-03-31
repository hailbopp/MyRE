import { Store } from "MyRE/Models/Store";
import { AppAction } from "MyRE/Actions";
import { some, none } from "ts-option";
import { List } from "immutable";
import * as ProjectActions from 'MyRE/Actions/Projects';
import { ProjectListing } from "MyRE/Api/Models";
import { parseSource, convertInternalSourceToDisplayFormat, convertDisplaySourceToInternalFormat } from "MyRE/Utils/Helpers/Project";
import { filterDevices } from "MyRE/Utils/Helpers/Instance";

export const reduceProjects = (fullState: Store.All, action: AppAction): Store.Projects => {
    const state = fullState.projects;
    let newState = Object.assign({}, state);

    switch (action.type) {
        case 'API_RESPONSE':
            if (action.requestType === 'API_REQUEST_PROJECT_LIST') {
                if (action.response.result === 'success') {
                    newState.projects = some(List(action.response.data.toArray().map((p: ProjectListing): Store.Project => ({
                        projectId: p.ProjectId,
                        name: p.Name,
                        description: p.Description,
                        instanceId: p.InstanceId,
                        source: p.Source
                    }))));
                }
            } else if (action.requestType === 'API_CREATE_NEW_PROJECT') {
                if (action.response.result === "success") {
                    newState.createProjectModalOpen = false;
                    newState.createProjectMessage = none;
                    newState.newProject = { Name: '', Description: '', InstanceId: '' };
                } else {
                    newState.createProjectMessage = some<Store.AlertMessage>({
                        level: 'danger',
                        message: action.response.message.getOrElse(action.response.status.toString()),
                    });
                }
            } else if (action.requestType === 'API_TEST_PROJECT_SOURCE') {
                if (state.activeProject.isDefined) {
                    let newActive = Object.assign({}, state.activeProject.get);
                    if (action.response.result === 'error') {
                        newActive.editorStatusMessage = some({
                            level: 'danger',
                            message: JSON.stringify({
                                message: action.response.message.getOrElse(""),
                                status: action.response.status
                            }),
                        })
                    } else {
                        newActive.editorStatusMessage = some({
                            level: 'default',
                            message: JSON.stringify(action.response.data),
                        })
                    }    
                    newState.activeProject = some(newActive);
                }
            } else if (action.requestType === 'API_LOGOUT') {
                if (action.response.result === "success") {
                    newState.projects = none;
                }
            } else if (action.requestType === 'API_ATTEMPT_LOGIN') {
                if (action.response.result === "success") {
                    newState.projects = none;
                } else {
                    newState.projects = some(List([]));
                }
            } else if (action.requestType === 'API_REQUEST_INSTANCE_DEVICES_LIST') {
                // Force a refresh of the active project so that we can properly display human-readable devices
            }

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
            newState.createProjectMessage = none;
            return newState;

        case 'UI_CHANGE_PROJECT_SOURCE':
            if (state.activeProject.isDefined && state.activeProject.get.internal.projectId == action.projectId) {
                // Create new display source and then dehumanize it for internal
                let newSource = Object.assign({}, state.activeProject.get.display.source);
                newSource.Source = action.newSource;
                newSource.ExpressionTree = parseSource(action.newSource);
                let newDisplayProj = Object.assign({}, state.activeProject.get.display);
                newDisplayProj.source = newSource;

                let newInternalSource = convertDisplaySourceToInternalFormat(newSource, action.newSource, action.availableDevices);
                let newInternalProj = Object.assign({}, newDisplayProj);
                newInternalProj.source = newInternalSource;

                newState.activeProject = some({
                    display: newDisplayProj,
                    internal: newInternalProj,
                    editorStatusMessage: state.activeProject.get.editorStatusMessage
                });
            }
            return newState;

        case 'UI_SET_ACTIVE_PROJECT':
            const internal = state.projects.getOrElse(List([])).find(p => !!p && p.projectId === action.projectId);
            if (!!internal) {
                let display = Object.assign({}, internal);
                display.source = convertInternalSourceToDisplayFormat(internal.source, action.availableDevices);

                newState.activeProject = some({
                    editorStatusMessage: none,
                    internal,
                    display,                    
                });
            }
            
            return newState;

        case 'UI_REFRESH_ACTIVE_PROJECT':
            if (state.activeProject.isDefined) {
                let internal = state.activeProject.get.internal;
                let display = Object.assign({}, internal);

                display.source = convertInternalSourceToDisplayFormat(internal.source, action.availableDevices);
                newState.activeProject = some({
                    editorStatusMessage: state.activeProject.get.editorStatusMessage,
                    internal,
                    display
                });
            }

            return newState;
            
        default:
            return state;
    }
}