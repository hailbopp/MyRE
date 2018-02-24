import { Atom } from "MyRE/Utils/Models/Atom";
import { SExpression } from "MyRE/Utils/Models/SExpression";

export type Expression =
    | string
    | number;

export type ExprTree =
    | Expression
    | Expression[];

export type Program = ExprTree;