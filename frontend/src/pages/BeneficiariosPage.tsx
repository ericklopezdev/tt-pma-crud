import React, { useState } from 'react';
import { useBeneficiarios } from '../hooks/useBeneficiarios';
import { BeneficiarioForm } from '../components/BeneficiarioForm';
import { BeneficiarioList } from '../components/BeneficiarioList';
import { CreateBeneficiarioDto, Beneficiario, UpdateBeneficiarioDto } from '../types';
import toast from 'react-hot-toast';
import { Plus, X, Users, LayoutGrid, List as ListIcon } from 'lucide-react';

export const BeneficiariosPage: React.FC = () => {
  const { beneficiarios, loading, createBeneficiario, updateBeneficiario, deleteBeneficiario } = useBeneficiarios();
  const [isFormVisible, setIsFormVisible] = useState(false);
  const [editingBeneficiario, setEditingBeneficiario] = useState<Beneficiario | null>(null);

  const handleCreate = async (data: CreateBeneficiarioDto) => {
    try {
      await createBeneficiario(data);
      setIsFormVisible(false);
      toast.success('Beneficiario creado exitosamente');
    } catch (error: any) {
      if (error.response?.data?.errors) {
        error.response.data.errors.forEach((err: any) => {
          toast.error(`${err.property}: ${err.error}`);
        });
      } else {
        toast.error('Error al crear el beneficiario');
      }
    }
  };

  const handleEdit = (beneficiario: Beneficiario) => {
    setEditingBeneficiario(beneficiario);
    setIsFormVisible(true);
  };

  const handleUpdate = async (id: number, data: UpdateBeneficiarioDto) => {
    try {
      await updateBeneficiario(id, data);
      setIsFormVisible(false);
      setEditingBeneficiario(null);
      toast.success('Beneficiario actualizado exitosamente');
    } catch (error: any) {
      if (error.response?.data?.errors) {
        error.response.data.errors.forEach((err: any) => {
          toast.error(`${err.property}: ${err.error}`);
        });
      } else {
        toast.error('Error al actualizar el beneficiario');
      }
    }
  };

  const handleDelete = async (id: number) => {
    if (window.confirm('¿Está seguro que desea eliminar este beneficiario?')) {
      try {
        await deleteBeneficiario(id);
        toast.success('Beneficiario eliminado exitosamente');
      } catch (error) {
        toast.error('Error al eliminar el beneficiario');
      }
    }
  };

  const handleFormSubmit = async (data: CreateBeneficiarioDto) => {
    if (editingBeneficiario) {
      await handleUpdate(editingBeneficiario.id, {
        ...data,
        estado: editingBeneficiario.estado
      });
    } else {
      await handleCreate(data);
    }
  };

  return (
    <div className="space-y-8 max-w-7xl mx-auto">
      <div className="flex flex-col md:flex-row justify-between items-start md:items-center gap-4 bg-white/50 dark:bg-slate-900/50 p-6 rounded-3xl backdrop-blur-md border border-white/20 dark:border-slate-800/50 shadow-sm">
        <div>
          <h1 className="text-3xl font-extrabold tracking-tight flex items-center gap-3">
            <Users className="text-primary-600" size={32} />
            Gestión de Beneficiarios
          </h1>
          <p className="text-slate-500 dark:text-slate-400 mt-1">
            Administra los beneficiarios de tu sistema de forma eficiente.
          </p>
        </div>
        <button
          onClick={() => {
            setEditingBeneficiario(null);
            setIsFormVisible(!isFormVisible);
          }}
          className={`btn-primary ${isFormVisible ? 'bg-slate-200 text-slate-900 dark:bg-slate-800 dark:text-slate-100 hover:bg-slate-300 dark:hover:bg-slate-700 shadow-none' : ''}`}
        >
          {isFormVisible ? (
            <><X size={20} /> Cancelar</>
          ) : (
            <><Plus size={20} /> Nuevo Beneficiario</>
          )}
        </button>
      </div>

      {isFormVisible && (
        <div className="card glass dark:glass-dark p-8 border-none animate-slide-up">
          <div className="flex items-center gap-2 mb-6">
            <div className="w-10 h-10 bg-primary-100 dark:bg-primary-900/30 rounded-full flex items-center justify-center text-primary-600">
              {editingBeneficiario ? <Plus className="rotate-45" size={24} /> : <Plus size={24} />}
            </div>
            <h2 className="text-2xl font-bold">
              {editingBeneficiario ? 'Editar Beneficiario' : 'Nuevo Beneficiario'}
            </h2>
          </div>
          <BeneficiarioForm
            initialData={editingBeneficiario ? {
              nombres: editingBeneficiario.nombres,
              apellidos: editingBeneficiario.apellidos,
              documentoIdentidadId: editingBeneficiario.documentoIdentidadId,
              numeroDocumento: editingBeneficiario.numeroDocumento,
              fechaNacimiento: new Date(editingBeneficiario.fechaNacimiento).toISOString().split('T')[0]
            } : undefined}
            currentDocument={editingBeneficiario?.documentoIdentidad}
            onSubmit={handleFormSubmit}
          />
        </div>
      )}

      <div className="space-y-4">
        <div className="flex justify-between items-center px-2">
          <h2 className="text-xl font-bold flex items-center gap-2">
            <ListIcon size={20} className="text-primary-600" />
            Lista de Beneficiarios
          </h2>
          <div className="flex items-center gap-2 bg-slate-200/50 dark:bg-slate-800/50 p-1 rounded-lg">
            <button className="p-1 px-2 rounded bg-white dark:bg-slate-700 shadow-sm text-primary-600">
              <LayoutGrid size={16} />
            </button>
          </div>
        </div>

        <div className="card glass dark:glass-dark p-0 overflow-hidden border-none shadow-xl">
          <BeneficiarioList
            beneficiarios={beneficiarios}
            onEdit={handleEdit}
            onDelete={handleDelete}
            isLoading={loading}
          />
        </div>
      </div>
    </div>
  );
};
