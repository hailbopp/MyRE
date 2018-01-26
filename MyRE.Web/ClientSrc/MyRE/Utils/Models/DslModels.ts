import { Atom } from "MyRE/Utils/Models/Atom";
import { SExpression } from "MyRE/Utils/Models/SExpression";

export type Expression =
    | SExpression
    | Atom;

export type Program = Expression[];