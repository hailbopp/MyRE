import { User, ErrorResponse, ProjectListing, Instance, CreateProjectRequest, Routine, DeviceInfo, DeviceState, ResultResponse, SmartThingsApiResponse } from "MyRE/Api/Models";
import { Option, some, none } from "ts-option";
import { ApiResult, ApiSuccess, ApiError } from "MyRE/Api/Models/Results";
import { List } from "immutable";
import { convertArrayToImmutableList } from "MyRE/Api/Utilities";

type QueryStringParameterGroup = any;

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
        updateProject: (project: ProjectListing) => `/api/Projects/${project.ProjectId}`,
        deleteProject: (projectId: string) => `/api/Projects/${projectId}`,
        executeProject: (projectId: string) => `/api/Projects/${projectId}/execute`,

        listDevices: '/api/Devices',
        getDeviceState: (deviceId: string) => `/api/Devices/${deviceId}`,
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

    private async performRequest<T>(input: RequestInfo, init: RequestInit, querystringParameters?: QueryStringParameterGroup): Promise<ApiResult<T>> {
        let qs = new URLSearchParams();
        if (querystringParameters) {
            for (let k in querystringParameters) {
                if (!querystringParameters.hasOwnProperty(k)) continue;
                qs.append(k, querystringParameters[k]);
            }
        }

        let response = await fetch(querystringParameters ? `${input}?${qs}` : input, init);
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

    private async get<T>(input: RequestInfo, querystringParameters?: QueryStringParameterGroup): Promise<ApiResult<T>> {
        let init: RequestInit = {
            credentials: 'include',
        };

        return await this.performRequest<T>(input, init, querystringParameters);
    }

    private prepareRequest(method: string, body?: any): RequestInit {
        let init: RequestInit = {
            body: !!body ? JSON.stringify(body) : undefined,
            method: method,
            headers: {
                "Content-Type": "application/json"
            },
            credentials: 'include',
        };

        return init
    }

    private async post<T>(input: RequestInfo, body?: any, querystringParameters?: QueryStringParameterGroup): Promise<ApiResult<T>> {
        let init = this.prepareRequest("POST", body);
        return await this.performRequest<T>(input, init, querystringParameters);
    }

    private async put<T>(input: RequestInfo, body: any, querystringParameters?: QueryStringParameterGroup): Promise<ApiResult<T>> {
        let init = this.prepareRequest("PUT", body);
        return await this.performRequest<T>(input, init, querystringParameters);
    }

    private async delete(input: RequestInfo, querystringParameters?: QueryStringParameterGroup): Promise<ApiResult<any>> {
        let init = this.prepareRequest("DELETE");
        return await this.performRequest<any>(input, init, querystringParameters);
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

    public updateProject = async (updatedEntity: ProjectListing): Promise<ApiResult<ProjectListing>> =>
        this.put<ProjectListing>(this.paths.updateProject(updatedEntity), updatedEntity);

    public deleteProject = async (projectId: string): Promise<ApiResult<any>> =>
        this.delete(this.paths.deleteProject(projectId));
    
    public executeProject = async (projectId: string): Promise<ApiResult<SmartThingsApiResponse<ResultResponse>>> =>
        this.post<SmartThingsApiResponse<ResultResponse>>(this.paths.executeProject(projectId));
    
    public listInstanceDevices = async (instanceId: string): Promise<ApiResult<List<DeviceInfo>>> => 
        this.get<Array<DeviceInfo>>(this.paths.listDevices, { instanceId }).then(convertArrayToImmutableList);
    
    public getDeviceState = async (deviceId: string): Promise<ApiResult<DeviceState>> =>
        this.get<DeviceState>(this.paths.getDeviceState(deviceId));
}
