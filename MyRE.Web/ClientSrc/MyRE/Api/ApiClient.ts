import { User, ErrorResponse, ProjectListing } from "MyRE/Api/Models";
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

        listProjects: '/api/Projects',
    }

    private getEndpointUrl<S extends keyof typeof MyREApiClient.paths>(pathName: S) {
        return this.baseUri + MyREApiClient.paths[pathName];
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

    private async performRequest<T>(input: RequestInfo, init: RequestInit): Promise<ApiResult<T>> {
        let response = await fetch(input, init);
        if (response.ok) {
            let entity: T = <T>(await response.json());
            return new ApiSuccess(entity);
        } else {
            let message = (await this.parseError(response)).Message;
            return new ApiError(response.status, some(message));
        }
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


        
    public logIn = async (email: string, password: string): Promise<ApiResult<undefined>> => this.post<undefined>(this.getEndpointUrl('logIn'), { Email: email, Password: password });    

    public logOut = async (): Promise<ApiResult<undefined>> => this.post<undefined>(this.getEndpointUrl("logOut"));

    public register = async (email: string, password: string): Promise<ApiResult<undefined>> => this.post<undefined>(this.getEndpointUrl('register'), { Email: email, Password: password });

    public getCurrentUser = async (): Promise<ApiResult<User>> => this.get<User>(this.getEndpointUrl('getCurrentUser'));

    public listProjects = async (): Promise<ApiResult<ProjectListing[]>> => this.get<ProjectListing[]>(this.getEndpointUrl('listProjects'));
}