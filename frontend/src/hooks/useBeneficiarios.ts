import { useState, useEffect } from 'react';
import { Beneficiario, CreateBeneficiarioDto, UpdateBeneficiarioDto, EstadoEnum } from '../types';
import { beneficiarioService } from '../services/beneficiarioService';

export const useBeneficiarios = () => {
  const [beneficiarios, setBeneficiarios] = useState<Beneficiario[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  const fetchBeneficiarios = async () => {
    try {
      setLoading(true);
      setError(null);
      const data = await beneficiarioService.getAll();
      setBeneficiarios(data.filter(b => b.estado === EstadoEnum.Activo));
    } catch (err) {
      setError('Error al cargar los beneficiarios');
      console.error('Error fetching beneficiaries:', err);
    } finally {
      setLoading(false);
    }
  };

  const createBeneficiario = async (data: CreateBeneficiarioDto) => {
    try {
      const newBeneficiario = await beneficiarioService.create(data);
      setBeneficiarios(prev => [...prev, newBeneficiario]);
      return newBeneficiario;
    } catch (err) {
      setError('Error al crear el beneficiario');
      throw err;
    }
  };

  const updateBeneficiario = async (id: number, data: UpdateBeneficiarioDto) => {
    try {
      await beneficiarioService.update(id, data);
      setBeneficiarios(prev =>
        prev.map(b => b.id === id ? { ...b, ...data } : b)
      );
    } catch (err) {
      setError('Error al actualizar el beneficiario');
      throw err;
    }
  };

  const deleteBeneficiario = async (id: number) => {
    try {
      await beneficiarioService.delete(id);
      setBeneficiarios(prev => prev.filter(b => b.id !== id));
    } catch (err) {
      setError('Error al eliminar el beneficiario');
      throw err;
    }
  };

  const getBeneficiarioById = async (id: number): Promise<Beneficiario> => {
    try {
      return await beneficiarioService.getById(id);
    } catch (err) {
      setError('Error al obtener el beneficiario');
      throw err;
    }
  };

  useEffect(() => {
    fetchBeneficiarios();
  }, []);

  return {
    beneficiarios,
    loading,
    error,
    createBeneficiario,
    updateBeneficiario,
    deleteBeneficiario,
    getBeneficiarioById,
    refetch: fetchBeneficiarios
  };
};