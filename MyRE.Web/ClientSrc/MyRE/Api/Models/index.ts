
export interface ErrorResponse {
    Message: string;
}

export interface User {
    UserId: string;    
    Email: string;
}

export interface Instance {
    Id: number;
    Name: string;
    AccountId: number;
}

export interface ProjectListing {
    ProjectId: number;
    Name: string;
    Description: string;
}
