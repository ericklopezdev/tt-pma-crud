import React from 'react';
import { useForm } from 'react-hook-form';
import { CreateBeneficiarioDto, DocumentoIdentidad } from '../types';
import { useDocumentTypes } from '../hooks/useDocumentTypes';
import { User, FileText, Calendar, ShieldCheck } from 'lucide-react';

interface BeneficiarioFormProps {
  initialData?: CreateBeneficiarioDto;
  currentDocument?: DocumentoIdentidad;
  onSubmit: (data: CreateBeneficiarioDto) => void;
  isLoading?: boolean;
}

export const BeneficiarioForm: React.FC<BeneficiarioFormProps> = ({
  initialData,
  currentDocument,
  onSubmit,
  isLoading = false
}) => {
  const { documents: fetchedDocuments } = useDocumentTypes();
  const { register, handleSubmit, watch, reset, formState: { errors } } = useForm<CreateBeneficiarioDto>({
    defaultValues: initialData
  });

  React.useEffect(() => {
    if (initialData) {
      reset(initialData);
    }
  }, [initialData, reset]);

  const documents = React.useMemo(() => {
    if (!currentDocument) return fetchedDocuments;

    // Check if current document is already in the list
    const exists = fetchedDocuments.some(d => d.id === currentDocument.id);
    if (exists) return fetchedDocuments;

    // Add current document to the list
    return [...fetchedDocuments, currentDocument];
  }, [fetchedDocuments, currentDocument]);

  const selectedDocumentId = watch('documentoIdentidadId');
  const selectedDocument = documents.find(d => d.id === selectedDocumentId);

  const validateDocumentNumber = (value: string) => {
    if (!selectedDocument) return true;

    if (value.length !== selectedDocument.longitud) {
      return `El documento debe tener ${selectedDocument.longitud} caracteres`;
    }

    if (selectedDocument.soloNumeros && !/^\d+$/.test(value)) {
      return 'El documento solo debe contener números';
    }

    return true;
  };

  return (
    <form onSubmit={handleSubmit(onSubmit)} className="space-y-6">
      <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
        <div className="space-y-2">
          <label className="text-sm font-bold text-slate-700 dark:text-slate-300 flex items-center gap-2">
            <User size={16} className="text-primary-600" />
            Nombres *
          </label>
          <div className="relative">
            <input
              {...register('nombres', {
                required: 'Los nombres son requeridos',
                maxLength: { value: 100, message: 'Máximo 100 caracteres' }
              })}
              className={`input-field ${errors.nombres ? 'border-rose-500 focus:ring-rose-500/20' : ''}`}
              placeholder="Ej. Juan Andrés"
            />
          </div>
          {errors.nombres && (
            <p className="text-rose-500 text-xs font-medium pl-1">{errors.nombres.message}</p>
          )}
        </div>

        <div className="space-y-2">
          <label className="text-sm font-bold text-slate-700 dark:text-slate-300 flex items-center gap-2">
            <User size={16} className="text-primary-600" />
            Apellidos *
          </label>
          <input
            {...register('apellidos', {
              required: 'Los apellidos son requeridos',
              maxLength: { value: 100, message: 'Máximo 100 caracteres' }
            })}
            className={`input-field ${errors.apellidos ? 'border-rose-500 focus:ring-rose-500/20' : ''}`}
            placeholder="Ej. Pérez García"
          />
          {errors.apellidos && (
            <p className="text-rose-500 text-xs font-medium pl-1">{errors.apellidos.message}</p>
          )}
        </div>

        <div className="space-y-2">
          <label className="text-sm font-bold text-slate-700 dark:text-slate-300 flex items-center gap-2">
            <ShieldCheck size={16} className="text-primary-600" />
            Tipo de Documento *
          </label>
          <select
            {...register('documentoIdentidadId', {
              required: 'Debe seleccionar un tipo de documento',
              valueAsNumber: true
            })}
            className={`input-field appearance-none cursor-pointer ${errors.documentoIdentidadId ? 'border-rose-500 focus:ring-rose-500/20' : ''}`}
          >
            <option value="">Seleccione un tipo...</option>
            {documents.map(doc => (
              <option key={doc.id} value={doc.id}>
                {doc.nombre} ({doc.codigo})
              </option>
            ))}
          </select>
          {errors.documentoIdentidadId && (
            <p className="text-rose-500 text-xs font-medium pl-1">{errors.documentoIdentidadId.message}</p>
          )}
        </div>

        <div className="space-y-2">
          <label className="text-sm font-bold text-slate-700 dark:text-slate-300 flex items-center gap-2">
            <FileText size={16} className="text-primary-600" />
            Número de Documento *
          </label>
          <input
            {...register('numeroDocumento', {
              required: 'El número de documento es requerido',
              validate: validateDocumentNumber
            })}
            className={`input-field ${errors.numeroDocumento ? 'border-rose-500 focus:ring-rose-500/20' : ''}`}
            placeholder="Ingrese el número"
          />
          {errors.numeroDocumento && (
            <p className="text-rose-500 text-xs font-medium pl-1">{errors.numeroDocumento.message}</p>
          )}
          {selectedDocument && !errors.numeroDocumento && (
            <p className="text-slate-500 text-[10px] pl-1 h-0">
              Requerido: {selectedDocument.longitud} caracteres {selectedDocument.soloNumeros && '(Solo números)'}
            </p>
          )}
        </div>

        <div className="space-y-2 md:col-span-2">
          <label className="text-sm font-bold text-slate-700 dark:text-slate-300 flex items-center gap-2">
            <Calendar size={16} className="text-primary-600" />
            Fecha de Nacimiento *
          </label>
          <input
            {...register('fechaNacimiento', {
              required: 'La fecha de nacimiento es requerida',
            })}
            type="date"
            className={`input-field ${errors.fechaNacimiento ? 'border-rose-500 focus:ring-rose-500/20' : ''}`}
          />
          {errors.fechaNacimiento && (
            <p className="text-rose-500 text-xs font-medium pl-1">{errors.fechaNacimiento.message}</p>
          )}
        </div>
      </div>

      <div className="pt-4 border-t border-slate-100 dark:border-slate-800">
        <button
          type="submit"
          disabled={isLoading}
          className="btn-primary w-full disabled:opacity-50 disabled:scale-100 h-12 text-lg shadow-xl shadow-primary-500/20"
        >
          {isLoading ? (
            <div className="flex items-center gap-2">
              <div className="w-4 h-4 border-2 border-white/30 border-t-white rounded-full animate-spin"></div>
              Guardando...
            </div>
          ) : 'Guardar Beneficiario'}
        </button>
      </div>
    </form>
  );
};
