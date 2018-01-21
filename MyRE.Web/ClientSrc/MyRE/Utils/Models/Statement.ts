import { OnStatement, ExpressionStatement } from "MyRE/Utils/Models/StatementTypes";

export type CoreStatement =
    | ExpressionStatement;

export type TopLevelStatement =
    | OnStatement;

export type Statement =
    | CoreStatement
    | TopLevelStatement