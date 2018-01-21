import { InitialExpression, Expression, Identifier } from "MyRE/Utils/Models/Expression";

export interface MemberAccessExpression {
    kind: 'EXPR_MEMBER_ACCESS'
    parent: Expression;
    property: Identifier;
}
