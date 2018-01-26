import { Atom } from "MyRE/Utils/Models/Atom";
import { Expression } from "MyRE/Utils/Models/DslModels";

export interface ListLiteral {
    type: 'LITERAL_LIST';
    value: Atom[];
}

export interface Invocation {
    type: 'S-EXPR';
    func: Atom;
    args: Atom[];
}

export interface ArgumentList {
    type: 'ARGUMENT_LIST';
    value: Atom[];
}

export interface VariableDefinition {
    type: 'VARIABLE_DEFINITION';
    name: string;
    value: Expression;
}

export interface NamedFunctionDefinition {
    type: 'NAMED_FUNCTION_DEFINITION';
    name: string;
    args: ArgumentList;
    body: Expression[];
}

export interface EventHandler {
    type: 'EVENT_HANDLER_DEFINITION';
    name: string;
    event: Expression;
    body: Expression;
}

export interface LambdaExpression {
    type: 'ANONYMOUS_FUNCTION_DEFINITION';
    args: ArgumentList;
    body: Expression[];
}

export interface GetProperty {
    type: 'GET_PROPERTY_DEFINITION';
    object: Expression;
    property: Expression;
}

export type SExpression =
    | Invocation
    | ListLiteral;