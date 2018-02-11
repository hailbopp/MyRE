import brace = require('brace')

declare module 'brace' {
    export function define(name: string, deps: string[], module: (acequire: any, exports: any, module: any) => any): void;

    export interface IEditSession {
        setMode(mode: object): void;
    }

    export interface TokenInfo {
        index: number;
        start: number;
        type: string;
        value: string;
    }

    export interface TokenIterator {
        stepBackward(): TokenInfo;
        stepForward(): TokenInfo;
        getCurrentToken(): TokenInfo;
        getCurrentTokenRow(): number;
        getCurrentTokenColumn(): number;
    }
}