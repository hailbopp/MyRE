import { SExpression } from "MyRE/Utils/Models/SExpression";


export interface BooleanAtom {
    type: 'LITERAL_BOOL';
    value: boolean;
}

export interface IntegerAtom {
    type: "LITERAL_INT";
    value: number;
}

export interface FloatAtom {
    type: "LITERAL_FLOAT";
    value: number;
}

export interface StringAtom {
    type: 'LITERAL_STRING';
    value: string;
}

export interface SymbolAtom {
    type: 'LITERAL_SYMBOL';
    value: string;
}

export interface ReferenceAtom {
    type: 'LITERAL_REFERENCE';
    value: string;
}

export interface CommentAtom {
    type: 'COMMENT';
    contents: string;
}

export type Atom =
    | SExpression
    | BooleanAtom
    | IntegerAtom
    | FloatAtom
    | StringAtom
    | SymbolAtom
    | ReferenceAtom
    | CommentAtom;