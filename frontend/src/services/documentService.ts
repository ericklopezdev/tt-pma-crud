import { api } from './api';
import { DocumentoIdentidad } from '../types';

export const documentService = {
  async getActiveDocuments(): Promise<DocumentoIdentidad[]> {
    const response = await api.get<DocumentoIdentidad[]>('/DocumentosIdentidad');
    return response.data;
  }
};