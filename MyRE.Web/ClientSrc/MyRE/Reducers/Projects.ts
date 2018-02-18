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
                newState.activeProject = none;
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
            if (state.activeProject.isDefined && state.activeProject.get.projectId == action.projectId) {
                let newSource = Object.assign({}, state.activeProject.get.source);
                newSource.Source = action.newSource;
                newSource.ExpressionTree = parseSource(action.newSource);

                let newProj = Object.assign({}, state.activeProject.get);
                newProj.displaySource = newSource;

                let newInternalSource = convertDisplaySourceToInternalFormat(newProj.displaySource, filterDevices(newProj.instanceId, fullState.instanceState));
                newProj.source = newInternalSource;

                newState.activeProject = some(newProj);
            }
            return newState;

        case 'UI_SET_ACTIVE_PROJECT':
            const project = state.projects.getOrElse(List([])).find(p => !!p && p.projectId === action.projectId);
            if (!!project) {
                let newDisplaySource = Object.assign({}, project.source);

                let newActiveProject =
                    Object.assign({
                        displaySource: convertInternalSourceToDisplayFormat(newDisplaySource, filterDevices(project.instanceId, fullState.instanceState))
                    }, JSON.parse(JSON.stringify(project)) as Store.Project);

                newActiveProject.displaySource
                newState.activeProject = some(newActiveProject);
            }
            return newState;
            
        default:
            return state;
    }
}