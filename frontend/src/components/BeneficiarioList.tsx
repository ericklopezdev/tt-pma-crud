import React from 'react';
import { Beneficiario } from '../types';
import { Edit2, Trash2, User, Calendar, FileText, CheckCircle2, XCircle } from 'lucide-react';

interface BeneficiarioListProps {
  beneficiarios: Beneficiario[];
  onEdit: (beneficiario: Beneficiario) => void;
  onDelete: (id: number) => void;
  isLoading?: boolean;
}

export const BeneficiarioList: React.FC<BeneficiarioListProps> = ({
  beneficiarios,
  onEdit,
  onDelete,
  isLoading = false
}) => {
  const formatDate = (dateString: string) => {
    return new Date(dateString).toLocaleDateString('es-ES', {
      year: 'numeric',
      month: 'short',
      day: 'numeric'
    });
  };

  const getEstadoBadge = (estado: number) => {
    return estado === 1
      ? 'bg-emerald-100 text-emerald-700 dark:bg-emerald-900/30 dark:text-emerald-400 border-emerald-200 dark:border-emerald-800'
      : 'bg-rose-100 text-rose-700 dark:bg-rose-900/30 dark:text-rose-400 border-rose-200 dark:border-rose-800';
  };

  if (isLoading) {
    return (
      <div className="flex flex-col justify-center items-center h-64 gap-4">
        <div className="animate-spin rounded-full h-12 w-12 border-4 border-primary-200 border-t-primary-600"></div>
        <p className="text-slate-500 font-medium">Cargando beneficiarios...</p>
      </div>
    );
  }

  if (beneficiarios.length === 0) {
    return (
      <div className="text-center py-20 px-4">
        <div className="bg-slate-100 dark:bg-slate-800 w-16 h-16 rounded-full flex items-center justify-center mx-auto mb-4 text-slate-400">
          <User size={32} />
        </div>
        <h3 className="text-lg font-bold text-slate-900 dark:text-slate-100">No hay beneficiarios</h3>
        <p className="text-slate-500 dark:text-slate-400">Comienza agregando uno nuevo al sistema.</p>
      </div>
    );
  }

  return (
    <div className="overflow-x-auto">
      <table className="w-full border-collapse">
        <thead>
          <tr className="bg-slate-50/50 dark:bg-slate-800/50 border-b border-slate-100 dark:border-slate-800 text-slate-500 dark:text-slate-400 text-xs uppercase tracking-wider">
            <th className="text-left p-4 font-bold">Beneficiario</th>
            <th className="text-left p-4 font-bold">Documento</th>
            <th className="text-left p-4 font-bold">Fecha Nac.</th>
            <th className="text-left p-4 font-bold">Estado</th>
            <th className="text-right p-4 font-bold">Acciones</th>
          </tr>
        </thead>
        <tbody className="divide-y divide-slate-100 dark:divide-slate-800">
          {beneficiarios.map((beneficiario) => (
            <tr key={beneficiario.id} className="hover:bg-slate-50/50 dark:hover:bg-slate-800/50 transition-colors group">
              <td className="p-4">
                <div className="flex items-center gap-3">
                  <div className="w-10 h-10 rounded-full bg-primary-100 dark:bg-primary-900/40 text-primary-600 flex items-center justify-center font-bold">
                    {beneficiario.nombres.charAt(0)}{beneficiario.apellidos.charAt(0)}
                  </div>
                  <div>
                    <div className="font-bold text-slate-900 dark:text-slate-100">{beneficiario.nombres} {beneficiario.apellidos}</div>
                    <div className="text-xs text-slate-500 flex items-center gap-1">
                      <User size={10} /> ID: {beneficiario.id}
                    </div>
                  </div>
                </div>
              </td>
              <td className="p-4">
                <div className="space-y-1">
                  <div className="text-sm font-medium flex items-center gap-1.5">
                    <FileText size={14} className="text-slate-400" />
                    {beneficiario.documentoIdentidad?.nombre || '-'}
                  </div>
                  <div className="text-xs text-slate-500 ml-5">{beneficiario.numeroDocumento}</div>
                </div>
              </td>
              <td className="p-4">
                <div className="text-sm flex items-center gap-1.5 group-hover:text-primary-600 transition-colors">
                  <Calendar size={14} className="text-slate-400" />
                  {formatDate(beneficiario.fechaNacimiento)}
                </div>
              </td>
              <td className="p-4">
                <span className={`inline-flex items-center gap-1 px-3 py-1 rounded-full text-xs font-bold border ${getEstadoBadge(beneficiario.estado)}`}>
                  {beneficiario.estado === 1 ? (
                    <><CheckCircle2 size={12} /> Activo</>
                  ) : (
                    <><XCircle size={12} /> Inactivo</>
                  )}
                </span>
              </td>
              <td className="p-4">
                <div className="flex justify-end gap-2">
                  <button
                    onClick={() => onEdit(beneficiario)}
                    className="p-2 text-primary-600 hover:bg-primary-50 dark:hover:bg-primary-900/30 rounded-lg transition-all active:scale-90"
                    title="Editar"
                  >
                    <Edit2 size={18} />
                  </button>
                  <button
                    onClick={() => onDelete(beneficiario.id)}
                    className="p-2 text-rose-600 hover:bg-rose-50 dark:hover:bg-rose-900/30 rounded-lg transition-all active:scale-90"
                    title="Eliminar"
                  >
                    <Trash2 size={18} />
                  </button>
                </div>
              </td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
};
