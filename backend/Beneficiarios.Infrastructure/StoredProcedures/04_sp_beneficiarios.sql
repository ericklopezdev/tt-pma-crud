-- 04_sp_beneficiarios.sql
-- Stored Procedures para Beneficiarios

-- Obtener todos los beneficiarios con información del documento
CREATE PROCEDURE SP_BENEFICIARIOS_GET_ALL
AS
BEGIN
    SELECT 
        b.Id,
        b.Nombres,
        b.Apellidos,
        b.DocumentoIdentidadId,
        b.NumeroDocumento,
        b.FechaNacimiento,
        b.Estado,
        di.Id AS DocumentoId,
        di.Nombre AS DocumentoNombre,
        di.Codigo AS DocumentoCodigo,
        di.Longitud AS DocumentoLongitud,
        di.SoloNumeros AS DocumentoSoloNumeros,
        di.Estado AS DocumentoEstado
    FROM Beneficiario b
    LEFT JOIN DocumentoIdentidad di ON b.DocumentoIdentidadId = di.Id
    ORDER BY b.Apellidos, b.Nombres;
END;
GO

-- Obtener beneficiario por ID con información del documento
CREATE PROCEDURE SP_BENEFICIARIOS_GET_BY_ID
    @Id INT
AS
BEGIN
    SELECT 
        b.Id,
        b.Nombres,
        b.Apellidos,
        b.DocumentoIdentidadId,
        b.NumeroDocumento,
        b.FechaNacimiento,
        b.Estado,
        di.Id AS DocumentoId,
        di.Nombre AS DocumentoNombre,
        di.Codigo AS DocumentoCodigo,
        di.Longitud AS DocumentoLongitud,
        di.SoloNumeros AS DocumentoSoloNumeros,
        di.Estado AS DocumentoEstado
    FROM Beneficiario b
    LEFT JOIN DocumentoIdentidad di ON b.DocumentoIdentidadId = di.Id
    WHERE b.Id = @Id;
END;
GO

-- Insertar nuevo beneficiario
CREATE PROCEDURE SP_BENEFICIARIOS_INSERT
    @Nombres NVARCHAR(100),
    @Apellidos NVARCHAR(100),
    @DocumentoIdentidadId INT,
    @NumeroDocumento NVARCHAR(50),
    @FechaNacimiento DATE,
    @Id INT OUTPUT
AS
BEGIN
    INSERT INTO Beneficiario (Nombres, Apellidos, DocumentoIdentidadId, NumeroDocumento, FechaNacimiento, Estado)
    VALUES (@Nombres, @Apellidos, @DocumentoIdentidadId, @NumeroDocumento, @FechaNacimiento, 1);
    
    SET @Id = SCOPE_IDENTITY();
END;
GO

-- Actualizar beneficiario
CREATE PROCEDURE SP_BENEFICIARIOS_UPDATE
    @Id INT,
    @Nombres NVARCHAR(100),
    @Apellidos NVARCHAR(100),
    @DocumentoIdentidadId INT,
    @NumeroDocumento NVARCHAR(50),
    @FechaNacimiento DATE,
    @Estado INT
AS
BEGIN
    UPDATE Beneficiario
    SET 
        Nombres = @Nombres,
        Apellidos = @Apellidos,
        DocumentoIdentidadId = @DocumentoIdentidadId,
        NumeroDocumento = @NumeroDocumento,
        FechaNacimiento = @FechaNacimiento,
        Estado = @Estado
    WHERE Id = @Id;
END;
GO

-- Eliminar beneficiario (cambio de estado a inactivo)
CREATE PROCEDURE SP_BENEFICIARIOS_DELETE
    @Id INT
AS
BEGIN
    UPDATE Beneficiario
    SET Estado = 0
    WHERE Id = @Id;
END;