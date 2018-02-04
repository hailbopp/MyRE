import { ApiResult, ApiSuccess } from "MyRE/Api/Models/Results";
import { List } from "immutable";

export const convertArrayToImmutableList = <T>(res: ApiResult<Array<T>>): ApiResult<List<T>> => {
        switch (res.result) {
            case 'error':
                return res;
            case 'success':
                return new ApiSuccess(List(res.data));
        }
    }