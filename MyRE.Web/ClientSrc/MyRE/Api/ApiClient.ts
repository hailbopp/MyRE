import { User, ErrorResponse, ProjectListing, Instance, CreateProjectRequest } from "MyRE/Api/Models";
import { JsonConvert } from "json2typescript";
import { Option, some, none } from "ts-option";
import { ApiResult, ApiSuccess, ApiError } from "MyRE/Api/Models/Results";

export class MyREApiClient {
    private baseUri: string;

    constructor(baseUri: string) {
        this.baseUri = baseUri;
    }

    private static paths = {
        logIn: '/api/Auth/Login',
        logOut: '/api/Auth/Logout',
        register: '/api/Auth/Register',

        getCurrentUser: '/api/Users/Me',
        listUserInstances: (userId: string) => `/api/Users/${userId}/Instances`,

        listProjects: '/api/Projects',
        createProject: '/api/Projects',
    }

    private getEndpointUrl<S extends keyof typeof MyREApiClient.paths>(pathName: S, ...args: string[]) {
        let path = MyREApiClient.paths[pathName];
        if (typeof path === 'string' || path instanceof String) {
            return this.baseUri + path;
        } else {
            let result = (<(...a: string[]) => string>path)(...args);
            return result;
        }        
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

        return this.performRequest<T>(input, init);
    }
    
    private async post<T>(input: RequestInfo, body?: any): Promise<ApiResult<T>> {
        let init: RequestInit = {};
        init.body = JSON.stringify(body);
        init.method = 'POST';
        init.headers = {};
        init.headers['Content-Type'] = 'application/json';
        init.credentials = 'include';

        return this.performRequest<T>(input, init);
    }
        
    public logIn = async (email: string, password: string): Promise<ApiResult<any>> =>
        this.post<any>(this.getEndpointUrl('logIn'), {
            Email: email, Password: password
        });

    public logOut = async (): Promise<ApiResult<any>> =>
        this.post<any>(this.getEndpointUrl("logOut"));

    public register = async (email: string, password: string): Promise<ApiResult<any>> =>
        this.post<any>(this.getEndpointUrl('register'), {
            Email: email, Password: password
        });

    public getCurrentUser = async (): Promise<ApiResult<User>> =>
        this.get<User>(this.getEndpointUrl('getCurrentUser'));

    public listUserInstances = async (userId: string): Promise<ApiResult<Instance[]>> =>
        this.get<Instance[]>(this.getEndpointUrl('listUserInstances', userId));

    public listProjects = async (): Promise<ApiResult<ProjectListing[]>> =>
        this.get<ProjectListing[]>(this.getEndpointUrl('listProjects'));

    public createProject = async (newEntity: CreateProjectRequest): Promise<ApiResult<ProjectListing>> =>
        this.post<ProjectListing>(this.getEndpointUrl('createProject'), newEntity);
}
