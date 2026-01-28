-- 01_create_tables.sql
-- Creación de las tablas para el Sistema de Gestión de Beneficiarios

-- Tabla de Documentos de Identidad
CREATE TABLE DocumentoIdentidad (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Nombre NVARCHAR(100) NOT NULL,
    Codigo NVARCHAR(10) NOT NULL UNIQUE,
    Longitud INT NOT NULL,
    SoloNumeros BIT NOT NULL DEFAULT 0,
    Estado INT NOT NULL DEFAULT 1 -- 1 = Activo, 0 = Inactivo
);

-- Tabla de Beneficiarios
CREATE TABLE Beneficiario (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Nombres NVARCHAR(100) NOT NULL,
    Apellidos NVARCHAR(100) NOT NULL,
    DocumentoIdentidadId INT NOT NULL,
    NumeroDocumento NVARCHAR(50) NOT NULL,
    FechaNacimiento DATE NOT NULL,
    Estado INT NOT NULL DEFAULT 1, -- 1 = Activo, 0 = Inactivo
    CONSTRAINT FK_Beneficiario_DocumentoIdentidad FOREIGN KEY (DocumentoIdentidadId) 
        REFERENCES DocumentoIdentidad(Id),
    CONSTRAINT UQ_Beneficiario_Documento UNIQUE (DocumentoIdentidadId, NumeroDocumento)
);

-- Índices para mejorar el rendimiento
CREATE INDEX IX_Beneficiario_DocumentoIdentidadId ON Beneficiario(DocumentoIdentidadId);
CREATE INDEX IX_Beneficiario_Estado ON Beneficiario(Estado);
CREATE INDEX IX_DocumentoIdentidad_Estado ON DocumentoIdentidad(Estado);