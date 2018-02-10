import { Program } from "MyRE/Utils/Models/DslModels";
import * as ParserTypes from "MyRE/Utils/Models/Parser";


export interface ErrorResponse {
    Message: string;
}

export interface User {
    UserId: string;    
    Email: string;
}

export interface Instance {
    InstanceId: string;
    Name: string;
    AccountId: string;
}

type ExpressionTree = Program | ParserTypes.SyntaxError;

export interface ProjectSource {
    ProjectSourceId: string;
    CreatedAt: Date;
    ProjectId: string;
    Source: string;
    ExpressionTree: ExpressionTree
}

export interface ProjectListing {
    ProjectId: string;
    Name: string;
    Description: string;
    InstanceId: string;
    Source: ProjectSource;
}

export interface CreateProjectRequest {
    Name: string;
    Description: string;
    InstanceId: string;
}

export interface Routine {
    RoutineId: string;
    Name: string;
    Description: string;
    ProjectId: string;
    BlockId: string;
    ExecutionMethod: string;
}

export interface AttributeInfo {
    Name: string;
    Type: string;
    Values: Array<string>;
}

export interface ArgumentDataType {
    EnumType: string;
    Name: string;
}

export interface CommandInfo {
    Name: string;
    Arguments: Array<ArgumentDataType>;
}

export interface CapabilityInfo {
    Name: string;
    Attributes: Array<AttributeInfo>;
    Commands: Array<CommandInfo>;
}

export interface DeviceInfo {
    DeviceId: string;
    Label: string;
    DisplayName: string;
    ModelName: string;
    Manufacturer: string;
    Attributes: Array<AttributeInfo>;
    Commands: Array<CommandInfo>;
    Capabilities: Array<CapabilityInfo>;
}
