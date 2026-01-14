export enum ApplicationStatus {
  Applied = 0,
  Interview = 1,
  Offer = 2,
  Rejected = 3,
}

export interface Application {
  id: string;
  companyName: string;
  jobTitle: string;
  status: ApplicationStatus;
  jobDescription: string;
  location?: string;
  salary?: number;
  appliedDate: string;
  interviewDate?: string;
  notes?: string;
  jobUrl?: string;
}

export interface CreateApplicationRequest {
  companyName: string;
  jobTitle: string;
  status: ApplicationStatus;
  jobDescription: string;
  location?: string;
  salary?: number;
  appliedDate: string;
  interviewDate?: string;
  notes?: string;
  jobUrl?: string;
}

export interface UpdateApplicationRequest {
  companyName?: string;
  jobTitle?: string;
  status?: ApplicationStatus;
  jobDescription?: string;
  location?: string;
  salary?: number;
  appliedDate: string;
  interviewDate?: string;
  notes?: string;
  jobUrl?: string;
}

// Alias for consistency
export type ApplicationUpdateDto = UpdateApplicationRequest;
