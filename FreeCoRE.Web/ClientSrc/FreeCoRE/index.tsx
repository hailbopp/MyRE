import * as React from 'react';
import * as ReactDOM from 'react-dom';
import { Provider } from 'react-redux';
import { BrowserRouter } from 'react-router-dom'
import { Store as ReduxStore, createStore, applyMiddleware, combineReducers } from 'redux';
import { Store } from 'FreeCoRE/Models/Store';
import { LoggerMiddleware } from 'FreeCoRE/Middleware/Logging';
import { reduce, initialState } from 'FreeCoRE/Reducers';
import { Application } from 'FreeCoRE/Components/Application';

export const APP_NAME = "FreeCoRE";
const globalStyles = require('./global.scss');

const middleware = [
    LoggerMiddleware,
]

export const store: ReduxStore<Store.All> = createStore(reduce, initialState, applyMiddleware(...middleware));

export const init = (debug: boolean = false) => {
    if (debug) {
        console.warn(`[${APP_NAME}] Debug mode enabled.`);
    }

    ReactDOM.render(
        <BrowserRouter>
            <Provider store={store}>
                <Application />
            </Provider>
        </BrowserRouter>
        , document.getElementById('freecoreBody'));
}