import { ProjectListing, CreateProjectRequest, Routine, DeviceInfo, ResultResponse } from "MyRE/Api/Models";
import { List } from "immutable";
import { ApiError } from "MyRE/Api/Models/Results";
import { ApiResponseAction } from "MyRE/Actions";

export type ProjectListRequestApiAction = {
    type: 'API_REQUEST_PROJECT_LIST';
    asyncActionType: 'API_REQUEST';
}

export type ProjectListResponseApiAction = ApiResponseAction<ProjectListRequestApiAction, List<ProjectListing>>;

export const requestProjectList = (): ProjectListRequestApiAction => ({
    type: 'API_REQUEST_PROJECT_LIST',
    asyncActionType: 'API_REQUEST',
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

export type CreateNewProjectRequestApiAction = {
    type: 'API_CREATE_NEW_PROJECT';
    asyncActionType: 'API_REQUEST';
    newProject: CreateProjectRequest;
}

export type CreateNewProjectResponseApiAction = ApiResponseAction<CreateNewProjectRequestApiAction, ProjectListing>;

export const createNewProject = (newProject: CreateProjectRequest): CreateNewProjectRequestApiAction => ({
    type: 'API_CREATE_NEW_PROJECT',
    asyncActionType: 'API_REQUEST',
    newProject,
});

export type UpdateProjectRequestApiAction = {
    type: 'API_UPDATE_PROJECT';
    asyncActionType: 'API_REQUEST';
    updatedEntity: ProjectListing;
}

export type UpdateProjectResponseApiAction = ApiResponseAction<UpdateProjectRequestApiAction, ProjectListing>;

export const updateProject = (updatedEntity: ProjectListing): UpdateProjectRequestApiAction => ({
    type: 'API_UPDATE_PROJECT',
    asyncActionType: 'API_REQUEST',
    updatedEntity,
});

export type DeleteProjectRequestApiAction = {
    type: 'API_DELETE_PROJECT';
    asyncActionType: 'API_REQUEST';
    projectId: string;
}

export type DeleteProjectResponseApiAction = ApiResponseAction<DeleteProjectRequestApiAction, null>;

export const deleteProject = (projectId: string): DeleteProjectRequestApiAction => ({
    type: 'API_DELETE_PROJECT',
    asyncActionType: 'API_REQUEST',
    projectId
});

export type TestProjectSourceRequestApiAction = {
    type: 'API_TEST_PROJECT_SOURCE';
    asyncActionType: 'API_REQUEST';
    instanceId: string;
    source: string;
}

export type TestProjectSourceResponseApiAction = ApiResponseAction<TestProjectSourceRequestApiAction, ResultResponse>;

export const testProjectSource = (instanceId: string, source: string): TestProjectSourceRequestApiAction => ({
    type: 'API_TEST_PROJECT_SOURCE',
    asyncActionType: 'API_REQUEST',
    instanceId,
    source
});

export type ChangeProjectSourceAction = {
    type: 'UI_CHANGE_PROJECT_SOURCE';
    projectId: string;
    availableDevices: DeviceInfo[];
    newSource: string;
}

export const changeProjectSource = (projectId: string, availableDevices: DeviceInfo[], source: string): ChangeProjectSourceAction => ({
    type: 'UI_CHANGE_PROJECT_SOURCE',
    projectId: projectId,
    availableDevices: availableDevices,
    newSource: source,
});

export type SetActiveProjectAction = {
    type: 'UI_SET_ACTIVE_PROJECT';
    availableDevices: DeviceInfo[];
    projectId: string;
}

export const setActiveProject = (projectId: string, availableDevices: DeviceInfo[]): SetActiveProjectAction => ({
    type: 'UI_SET_ACTIVE_PROJECT',
    availableDevices,
    projectId,
});

export type RefreshActiveProjectAction = {
    type: 'UI_REFRESH_ACTIVE_PROJECT';
    instanceId: string;
    availableDevices: DeviceInfo[];
}

export const refreshActiveProject = (instanceId: string, availableDevices: DeviceInfo[]): RefreshActiveProjectAction => ({
    type: 'UI_REFRESH_ACTIVE_PROJECT',
    instanceId,
    availableDevices
});

export type ProjectApiRequestAction =
    | ProjectListRequestApiAction
    | CreateNewProjectRequestApiAction
    | UpdateProjectRequestApiAction
    | DeleteProjectRequestApiAction
    | TestProjectSourceRequestApiAction

export type ProjectApiResponseAction =
    | ProjectListResponseApiAction
    | CreateNewProjectResponseApiAction
    | UpdateProjectResponseApiAction
    | DeleteProjectResponseApiAction
    | TestProjectSourceResponseApiAction

export type ProjectUIAction =
    | ToggleCreateProjectDialogUIAction
    | ChangeNewProjectDataUIAction
    | ChangeProjectSourceAction
    | SetActiveProjectAction
    | RefreshActiveProjectAction


export type ProjectAction =
    | ProjectApiRequestAction
    | ProjectApiResponseAction
    | ProjectUIAction