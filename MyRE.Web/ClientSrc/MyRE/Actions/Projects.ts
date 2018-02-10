import { ProjectListing, CreateProjectRequest, Routine } from "MyRE/Api/Models";
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

export type ChangeProjectSourceAction = {
    type: 'UI_CHANGE_PROJECT_SOURCE';
    projectId: string;
    newSource: string;
}

export const changeProjectSource = (projectId: string, source: string): ChangeProjectSourceAction => ({
    type: 'UI_CHANGE_PROJECT_SOURCE',
    projectId: projectId,
    newSource: source,
});

export type SetActiveProjectAction = {
    type: 'UI_SET_ACTIVE_PROJECT';
    projectId: string;
}

export const setActiveProject = (projectId: string): SetActiveProjectAction => ({
    type: 'UI_SET_ACTIVE_PROJECT',
    projectId,
});

export type ProjectApiRequestAction =
    | ProjectListRequestApiAction
    | CreateNewProjectRequestApiAction
    | UpdateProjectRequestApiAction
    | DeleteProjectRequestApiAction

export type ProjectApiResponseAction =
    | ProjectListResponseApiAction
    | CreateNewProjectResponseApiAction
    | UpdateProjectResponseApiAction
    | DeleteProjectResponseApiAction

export type ProjectUIAction =
    | ToggleCreateProjectDialogUIAction
    | ChangeNewProjectDataUIAction
    | ChangeProjectSourceAction
    | SetActiveProjectAction


export type ProjectAction =
    | ProjectApiRequestAction
    | ProjectApiResponseAction
    | ProjectUIAction