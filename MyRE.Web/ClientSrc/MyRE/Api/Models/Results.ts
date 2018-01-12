import { Option } from "ts-option";


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