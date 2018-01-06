import * as React from 'react';
import * as ReactDOM from 'react-dom';
import { Provider } from 'react-redux';
import { BrowserRouter as Router } from 'react-router-dom'
import { Store as ReduxStore, createStore, applyMiddleware, combineReducers, MiddlewareAPI } from 'redux';
import { Store } from 'MyRE/Models/Store';
import { LoggerMiddleware } from 'MyRE/Middleware/Logging';
import { reduce, initialState } from 'MyRE/Reducers';
import { Application } from 'MyRE/Components/Application';
import { ApiServiceMiddleware } from 'MyRE/Middleware/Api';

export const APP_NAME = "MyRE";
const globalStyles = require('./global.scss');

const middleware = [
    ApiServiceMiddleware,
    LoggerMiddleware,
]

export const store: ReduxStore<Store.All> = createStore(reduce, initialState, applyMiddleware(...middleware));

export const init = (debug: boolean = false) => {
    if (debug) {
        console.warn(`[${APP_NAME}] Debug mode enabled.`);
    }

    ReactDOM.render(
        <Router>
            <Provider store={store}>
                <Application />
            </Provider>
        </Router>
        , document.getElementById('MyREBody'));
}