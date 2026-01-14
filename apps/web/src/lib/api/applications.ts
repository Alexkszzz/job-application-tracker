import api from "./axios";
import {
  Application,
  CreateApplicationRequest,
  UpdateApplicationRequest,
} from "./types";

export const applicationService = {
  // Get all applications
  getAll: async (): Promise<Application[]> => {
    const { data } = await api.get<Application[]>("/applications");
    return data;
  },

  // Get application by ID
  getById: async (id: string): Promise<Application> => {
    const { data } = await api.get<Application>(`/applications/${id}`);
    return data;
  },

  // Create new application
  create: async (
    application: CreateApplicationRequest
  ): Promise<Application> => {
    const { data } = await api.post<Application>("/applications", application);
    return data;
  },

  // Update application
  update: async (
    id: string,
    application: UpdateApplicationRequest
  ): Promise<void> => {
    await api.put(`/applications/${id}`, application);
  },

  // Delete application
  delete: async (id: string): Promise<void> => {
    await api.delete(`/applications/${id}`);
  },
};
