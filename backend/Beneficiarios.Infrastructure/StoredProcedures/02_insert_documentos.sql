-- 02_insert_documentos.sql
-- Inserción de datos iniciales para Documentos de Identidad

INSERT INTO DocumentoIdentidad (Nombre, Codigo, Longitud, SoloNumeros, Estado) VALUES
('DNI', 'DNI', 8, 1, 1), -- 8 dígitos, solo números
('Pasaporte', 'PAS', 12, 0, 1), -- 12 caracteres, puede incluir letras
('Cédula de Extranjería', 'CE', 10, 1, 1), -- 10 dígitos, solo números
('Tarjeta de Identidad', 'TI', 8, 1, 1); -- 8 dígitos, solo números