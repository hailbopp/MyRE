import { User, ErrorResponse } from "FreeCoRE/Api/Models";
import { JsonConvert } from "json2typescript";
import { Option, some, none } from "ts-option";



export class ApiSuccess<T> {
    result: 'success';
    data: T;

    constructor(data: T) {
        this.result = 'success';
        this.data = data;
    }
}

export class ApiError {
    result: 'error';
    message: Option<string>;
    status: number;

    constructor(status: number, message: Option<string>) {
        this.result = 'error';
        this.message = message;
        this.status = status;
    }
}

export type ApiResult<T> =
    | ApiSuccess<T>
    | ApiError

export class FreeCoreApiClient {
    private baseUri: string;

    constructor(baseUri: string) {
        this.baseUri = baseUri;
    }

    private static paths = {
        logIn: '/api/Auth/Login',
        logOut: '/api/Auth/Logout',
        register: '/api/Auth/Register',

        getCurrentUser: '/api/Users/Me',
    }

    private getEndpointUrl<S extends keyof typeof FreeCoreApiClient.paths>(pathName: S) {
        return this.baseUri + FreeCoreApiClient.paths[pathName];
    }

    private get(input: RequestInfo): Promise<Response> {
        let init: RequestInit = {
            credentials: 'include',
        };

        return fetch(input, init);
    }
    
    private post(input: RequestInfo, body?: any): Promise<Response> {
        let init: RequestInit = {};
        init.body = JSON.stringify(body);
        init.method = 'POST';
        init.headers = {};
        init.headers['Content-Type'] = 'application/json';
        init.credentials = 'include';

        return fetch(input, init);
    }

    private async parseError(r: Response): Promise<ErrorResponse> {
        try {
            return <ErrorResponse>(await r.json());
        } catch(e) {
            return Promise.resolve({
                Message: r.statusText
            });
        }        
    }
        
    public logIn = (email: string, password: string): Promise<ApiResult<undefined>> => {
        return this.post(
            this.getEndpointUrl('logIn'), {
                Email: email,
                Password: password
            })
            .then(response => {
                if (response.status === 200) {
                    return new ApiSuccess(undefined);
                } else {
                    return new ApiError(response.status, some(response.statusText));
                }
            });
    }

    public logOut = (): Promise<ApiResult<undefined>> => 
        this.post(this.getEndpointUrl("logOut"))
            .catch(response => {
                return new ApiError(response.status, none);
            })
            .then(response => {
                return new ApiSuccess(undefined);
            })

    public register = (email: string, password: string): Promise<ApiResult<undefined>> =>
        this.post(this.getEndpointUrl('register'), { Email: email, Password: password })
            .then(async response => {
                if (response.ok) {
                    return new ApiSuccess(undefined);
                }
                let message = (await this.parseError(response)).Message;
                return new ApiError(response.status, some(message));
                
            });

    public getCurrentUser = (): Promise<ApiResult<User>> =>
        this.get(this.getEndpointUrl('getCurrentUser'))
            .then(async response => {
                if (response.ok) {
                    let user: User = <User>(await response.json());
                    return new ApiSuccess(user);
                } else {
                    let message = (await this.parseError(response)).Message;
                    return new ApiError(response.status, some(message));
                }
            });
}