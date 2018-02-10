import brace = require('brace')

declare module 'brace' {
    export function define(name: string, deps: string[], module: (acequire: any, exports: any, module: any) => any): void;
}