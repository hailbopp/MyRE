import { MemberAccessExpression } from "MyRE/Utils/Models/MemberAccessExpression";

export interface DeviceLiteral {
    kind: 'LITERAL_DEVICE';
    name: string;
}

export type Literal =
    | DeviceLiteral;

export interface Identifier {
    kind: 'IDENTIFIER';
    name: string;
}

export type InitialExpression =
    | Identifier
    | Literal;

export type Expression =
    | InitialExpression
    | MemberAccessExpression