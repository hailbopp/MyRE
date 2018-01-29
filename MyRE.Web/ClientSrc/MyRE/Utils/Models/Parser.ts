import { Program } from "MyRE/Utils/Models/DslModels";

export interface IFilePosition {
    offset: number;
    line: number;
    column: number;
}

export interface IFileRange {
    start: IFilePosition;
    end: IFilePosition;
}

interface ILiteralExpectation {
    type: "literal";
    text: string;
    ignoreCase: boolean;
}

interface IClassParts extends Array<string | IClassParts> { }

interface IClassExpectation {
    type: "class";
    parts: IClassParts;
    inverted: boolean;
    ignoreCase: boolean;
}

interface IAnyExpectation {
    type: "any";
}

interface IEndExpectation {
    type: "end";
}

interface IOtherExpectation {
    type: "other";
    description: string;
}

type Expectation = ILiteralExpectation | IClassExpectation | IAnyExpectation | IEndExpectation | IOtherExpectation;

export interface SyntaxError extends Error {
    message: string;
    expected: Expectation[];
    found: string | null;
    location: IFileRange;
    name: string;
}

export interface IParseOptions {
    filename?: string;
    startRule?: string;
    tracer?: any;
    [key: string]: any;
}

export type ParseFunction = (input: string, options: IParseOptions) => Program | SyntaxError;

export interface Parser {
    parse: ParseFunction;
}

export default Parser;