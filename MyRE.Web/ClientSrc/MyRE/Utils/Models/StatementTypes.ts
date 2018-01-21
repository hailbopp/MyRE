import { InitialExpression, Expression } from "MyRE/Utils/Models/Expression";
import { Block } from "MyRE/Utils/Models/Block";

export interface ExpressionStatement {
    kind: 'EXPR_STATEMENT';
    expr: Expression;
}

export interface OnStatement {
    kind: 'ON_STATEMENT';
    target: Expression;
    block: Block;
}