-- 03_sp_documentos.sql
-- Stored Procedures para Documentos de Identidad

-- Obtener todos los documentos de identidad
CREATE PROCEDURE SP_DOCUMENTOS_IDENTIDAD_GET_ALL
AS
BEGIN
    SELECT 
        Id,
        Nombre,
        Codigo,
        Longitud,
        SoloNumeros,
        Estado
    FROM DocumentoIdentidad
    ORDER BY Nombre;
END;
GO

-- Obtener documento de identidad por ID
CREATE PROCEDURE SP_DOCUMENTOS_IDENTIDAD_GET_BY_ID
    @Id INT
AS
BEGIN
    SELECT 
        Id,
        Nombre,
        Codigo,
        Longitud,
        SoloNumeros,
        Estado
    FROM DocumentoIdentidad
    WHERE Id = @Id;
END;
GO

-- Insertar nuevo documento de identidad
CREATE PROCEDURE SP_DOCUMENTOS_IDENTIDAD_INSERT
    @Nombre NVARCHAR(100),
    @Codigo NVARCHAR(10),
    @Longitud INT,
    @SoloNumeros BIT,
    @Id INT OUTPUT
AS
BEGIN
    INSERT INTO DocumentoIdentidad (Nombre, Codigo, Longitud, SoloNumeros, Estado)
    VALUES (@Nombre, @Codigo, @Longitud, @SoloNumeros, 1);
    
    SET @Id = SCOPE_IDENTITY();
END;
GO

-- Actualizar documento de identidad
CREATE PROCEDURE SP_DOCUMENTOS_IDENTIDAD_UPDATE
    @Id INT,
    @Nombre NVARCHAR(100),
    @Codigo NVARCHAR(10),
    @Longitud INT,
    @SoloNumeros BIT,
    @Estado INT
AS
BEGIN
    UPDATE DocumentoIdentidad
    SET 
        Nombre = @Nombre,
        Codigo = @Codigo,
        Longitud = @Longitud,
        SoloNumeros = @SoloNumeros,
        Estado = @Estado
    WHERE Id = @Id;
END;
GO

-- Eliminar documento de identidad (cambio de estado a inactivo)
CREATE PROCEDURE SP_DOCUMENTOS_IDENTIDAD_DELETE
    @Id INT
AS
BEGIN
    UPDATE DocumentoIdentidad
    SET Estado = 0
    WHERE Id = @Id;
END;