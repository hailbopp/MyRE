import { ProjectListing, CreateProjectRequest } from "MyRE/Api/Models";
import { List } from "immutable";
import { ApiError } from "MyRE/Api/Models/Results";

export type RequestProjectListApiAction = {
    type: 'API_REQUEST_PROJECT_LIST';
}

export type ReceivedProjectListApiAction = {
    type: 'API_RECEIVED_PROJECTS';
    projects: List<ProjectListing>;
}

export type FailedProjectListApiAction = {
    type: 'API_FAILED_PROJECT_LIST';
}

export const requestProjectList = (): RequestProjectListApiAction => ({
    type: 'API_REQUEST_PROJECT_LIST'
});

export type ToggleCreateProjectDialogUIAction = {
    type: 'UI_TOGGLE_CREATE_PROJECT_DIALOG';
}

export const toggleCreateProjectDialog = (): ToggleCreateProjectDialogUIAction => ({
    type: 'UI_TOGGLE_CREATE_PROJECT_DIALOG',
});

export type ChangeNewProjectDataUIAction = {
    type: 'UI_CHANGE_NEW_PROJECT_DATA';
    value: CreateProjectRequest;
}

export const changeNewProjectData = (value: CreateProjectRequest): ChangeNewProjectDataUIAction => ({
    type: 'UI_CHANGE_NEW_PROJECT_DATA',
    value,
})

export type CreateNewProjectApiAction = {
    type: 'API_CREATE_NEW_PROJECT';
    newProject: CreateProjectRequest;
}

export type SuccessfullyCreatedProjectApiAction = {
    type: 'API_SUCCESSFUL_CREATE_PROJECT';
    newProject: ProjectListing;
}

export type FailedCreateProjectApiAction = {
    type: 'API_FAILED_CREATE_PROJECT';
    error: ApiError;
}

export const createNewProject = (newProject: CreateProjectRequest): CreateNewProjectApiAction => ({
    type: 'API_CREATE_NEW_PROJECT',
    newProject,
});



export type ProjectAction =
    | RequestProjectListApiAction
    | ReceivedProjectListApiAction
    | FailedProjectListApiAction
    | ToggleCreateProjectDialogUIAction
    | ChangeNewProjectDataUIAction
    | CreateNewProjectApiAction
    | SuccessfullyCreatedProjectApiAction
    | FailedCreateProjectApiAction
