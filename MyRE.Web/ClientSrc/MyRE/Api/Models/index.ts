
export interface ErrorResponse {
    Message: string;
}

export interface User {
    UserId: string;    
    Email: string;
}

export interface ProjectListing {
    ProjectId: number;
    Name: string;
    Description: string;
}
