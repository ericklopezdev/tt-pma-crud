export enum EstadoEnum {
  Activo = 1,
  Inactivo = 0
}

export interface DocumentoIdentidad {
  id: number;
  nombre: string;
  codigo: string;
  longitud: number;
  soloNumeros: boolean;
  estado: EstadoEnum;
}

export interface Beneficiario {
  id: number;
  nombres: string;
  apellidos: string;
  documentoIdentidadId: number;
  numeroDocumento: string;
  fechaNacimiento: string;
  estado: EstadoEnum;
  documentoIdentidad?: DocumentoIdentidad;
}

export interface CreateBeneficiarioDto {
  nombres: string;
  apellidos: string;
  documentoIdentidadId: number;
  numeroDocumento: string;
  fechaNacimiento: string;
}

export interface UpdateBeneficiarioDto {
  nombres: string;
  apellidos: string;
  documentoIdentidadId: number;
  numeroDocumento: string;
  fechaNacimiento: string;
  estado: EstadoEnum;
}