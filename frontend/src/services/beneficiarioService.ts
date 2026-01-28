import { api } from './api';
import { Beneficiario, CreateBeneficiarioDto, UpdateBeneficiarioDto } from '../types';

export const beneficiarioService = {
  async getAll(): Promise<Beneficiario[]> {
    const response = await api.get<Beneficiario[]>('/beneficiarios');
    return response.data;
  },

  async getById(id: number): Promise<Beneficiario> {
    const response = await api.get<Beneficiario>(`/beneficiarios/${id}`);
    return response.data;
  },

  async create(data: CreateBeneficiarioDto): Promise<Beneficiario> {
    const response = await api.post<Beneficiario>('/beneficiarios', data);
    return response.data;
  },

  async update(id: number, data: UpdateBeneficiarioDto): Promise<void> {
    await api.put(`/beneficiarios/${id}`, data);
  },

  async delete(id: number): Promise<void> {
    await api.delete(`/beneficiarios/${id}`);
  }
};