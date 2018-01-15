
export interface ErrorResponse {
    Message: string;
}

export interface User {
    UserId: string;    
    Email: string;
}

export interface Instance {
    Id: string;
    Name: string;
    AccountId: string;
}

export interface ProjectListing {
    Id: string;
    Name: string;
    Description: string;
    InstanceId: string;
}

export interface CreateProjectRequest {
    Name: string;
    Description: string;
    InstanceId: string;
}
