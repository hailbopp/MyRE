import { ProjectListing } from "MyRE/Api/Models";
import { List } from "immutable";

export type RequestProjectListApiAction = {
    type: 'API_REQUEST_PROJECT_LIST';
}

export type ReceivedProjectListApiAction = {
    type: 'API_RECEIVED_PROJECTS';
    projects: List<ProjectListing>;
}

export const requestProjectList = (): RequestProjectListApiAction => ({
    type: 'API_REQUEST_PROJECT_LIST'
});