import { Store } from "MyRE/Models/Store";
import { AppAction } from "MyRE/Actions";
import { some } from "ts-option";

export const reduceProjects = (state: Store.Projects, action: AppAction): Store.Projects => {
    switch (action.type) {
        case 'API_REQUEST_PROJECT_LIST':
            return {
                projects: state.projects,
                retrievingProjects: true,
            };

        case 'API_RECEIVED_PROJECTS':
            return {
                projects: some(action.projects),
                retrievingProjects: false,
            };

        default:
            return state;
    }
}