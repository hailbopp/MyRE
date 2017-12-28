import { Middleware } from "redux";

//import { Action } from "../Actions";
//import { ThunkAction } from "redux-thunk";

export const LoggerMiddleware: Middleware = (store) => (next) => (action: any) => {
    console.log('dispatching', action);
    const result = next(action);
    console.log('new state', store.getState());
    return result;
};
