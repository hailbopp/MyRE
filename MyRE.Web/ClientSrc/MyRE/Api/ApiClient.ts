import { User, ErrorResponse, ProjectListing, Instance, CreateProjectRequest, Routine } from "MyRE/Api/Models";
import { Option, some, none } from "ts-option";
import { ApiResult, ApiSuccess, ApiError } from "MyRE/Api/Models/Results";
import { List } from "immutable";

const convertArrayToImmutableList = <T>(res: ApiResult<Array<T>>): ApiResult<List<T>> => {
    switch (res.result) {
        case 'error':
            return res;
        case 'success':
            return new ApiSuccess(List(res.data));
    }
}

export class MyREApiClient {
    private baseUri: string;

    constructor(baseUri: string) {
        this.baseUri = baseUri;
    }

    private paths = {
        logIn: '/api/Auth/Login',
        logOut: '/api/Auth/Logout',
        register: '/api/Auth/Register',

        getCurrentUser: '/api/Users/Me',
        listUserInstances: (userId: string) => `/api/Users/${userId}/Instances`,

        listProjects: '/api/Projects',
        createProject: '/api/Projects',
        deleteProject: (projectId: string) => `/api/Projects/${projectId}`,

        listProjectRoutines: (projectId: string) => `/api/Projects/${projectId}/Routines`,
    }

    private async parseError(r: Response): Promise<ErrorResponse> {
        try {
            return <ErrorResponse>(await r.json());
        } catch (e) {
            return Promise.resolve({
                Message: r.statusText
            });
        }
    }

    private async performRequest<T>(input: RequestInfo, init: RequestInit, noResponseType: boolean = false): Promise<ApiResult<T>> {
        let response = await fetch(input, init);
        if (response.ok) {
            try {
                let entity: T = <T>(await response.json());
                return new ApiSuccess(entity);
            } catch (e) {
                return new ApiSuccess({} as T);
            }
        }
        let message = (await this.parseError(response)).Message;
        return new ApiError(response.status, some(message));
    }

    private async get<T>(input: RequestInfo): Promise<ApiResult<T>> {
        let init: RequestInit = {
            credentials: 'include',
        };

        return await this.performRequest<T>(input, init);
    }

    private async post<T>(input: RequestInfo, body?: any): Promise<ApiResult<T>> {
        let init: RequestInit = {};
        init.body = JSON.stringify(body);
        init.method = 'POST';
        init.headers = {};
        init.headers['Content-Type'] = 'application/json';
        init.credentials = 'include';

        return await this.performRequest<T>(input, init);
    }

    private async delete(input: RequestInfo): Promise<ApiResult<any>> {
        let init: RequestInit = {
            credentials: 'include',
            method: 'DELETE',
        };

        return await this.performRequest<any>(input, init);
    }

    public logIn = async (email: string, password: string): Promise<ApiResult<any>> =>
        this.post<any>(this.paths.logIn, {
            Email: email, Password: password
        });

    public logOut = async (): Promise<ApiResult<any>> =>
        this.post<any>(this.paths.logOut);

    public register = async (email: string, password: string): Promise<ApiResult<any>> =>
        this.post<any>(this.paths.register, {
            Email: email, Password: password
        });

    public getCurrentUser = async (): Promise<ApiResult<User>> =>
        this.get<User>(this.paths.getCurrentUser);

    public listUserInstances = async (userId: string): Promise<ApiResult<List<Instance>>> =>
        this.get<Array<Instance>>(this.paths.listUserInstances(userId)).then(convertArrayToImmutableList);

    public listProjects = async (): Promise<ApiResult<List<ProjectListing>>> =>
        await this.get<Array<ProjectListing>>(this.paths.listProjects).then(convertArrayToImmutableList);

    public createProject = async (newEntity: CreateProjectRequest): Promise<ApiResult<ProjectListing>> =>
        this.post<ProjectListing>(this.paths.createProject, newEntity);

    public deleteProject = async (projectId: string): Promise<ApiResult<any>> =>
        this.delete(this.paths.deleteProject(projectId));

    public listProjectRoutines = async (projectId: string): Promise<ApiResult<List<Routine>>> =>
        this.get<Array<Routine>>(this.paths.listProjectRoutines(projectId)).then(convertArrayToImmutableList);
}
