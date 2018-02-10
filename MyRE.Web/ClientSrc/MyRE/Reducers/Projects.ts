import { Store } from "MyRE/Models/Store";
import { AppAction } from "MyRE/Actions";
import { some, none } from "ts-option";
import { List } from "immutable";
import * as ProjectActions from 'MyRE/Actions/Projects';
import { ProjectListing } from "MyRE/Api/Models";

import * as ParserTypes from 'MyRE/Utils/Models/Parser';
import { Program } from "MyRE/Utils/Models/DslModels";
var parser = require('MyRE/Utils/MyreLisp.pegjs') as ParserTypes.Parser;

export const reduceProjects = (state: Store.Projects, action: AppAction): Store.Projects => {
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
                try {
                    newSource.ExpressionTree = parser.parse(action.newSource, {});
                } catch (e) {
                    newSource.ExpressionTree = e;
                }

                let newProj = Object.assign({}, state.activeProject.get);
                newProj.source = newSource;

                newState.activeProject = some(newProj);
            }
            return newState;

        case 'UI_SET_ACTIVE_PROJECT':
            const project = state.projects.getOrElse(List([])).find(p => !!p && p.projectId === action.projectId);
            if (!!project) {
                newState.activeProject = some(JSON.parse(JSON.stringify(project)) as Store.Project);
            }
            return newState;
            
        default:
            return state;
    }
}