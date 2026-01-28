# API Documentation

## Endpoints Principales

### Documentos de Identidad
| Método | Endpoint | Descripción |
|--------|----------|-------------|
| GET | `/api/documentos-identidad` | Obtener todos los tipos de documento activos |

### Beneficiarios
| Método | Endpoint | Descripción |
|--------|----------|-------------|
| GET | `/api/beneficiarios` | Listar todos los beneficiarios |
| GET | `/api/beneficiarios/{id}` | Obtener beneficiario por ID |
| POST | `/api/beneficiarios` | Crear nuevo beneficiario |
| PUT | `/api/beneficiarios/{id}` | Actualizar beneficiario existente |
| DELETE | `/api/beneficiarios/{id}` | Eliminar beneficiario (soft delete) |

## Validaciones Implementadas

### Por Tipo de Documento
- **DNI**: 8 dígitos numéricos obligatorios
- **Pasaporte**: Longitud variable, caracteres alfanuméricos
- **Cédula**: Validación según país específico
- **Carnet de Identidad**: Formato configurable

### Validaciones Comunes
- Campos obligatorios: Nombres, Apellidos, Documento
- Formato de fecha válida
- Edad mínima/máxima configurable
- Sin duplicados por número de documento

## Testing de API

Para testing completo, usar el archivo:
**`docs/04-pruebas-api.http`**

Este archivo incluye:
- Todos los endpoints con ejemplos
- Casos de error y validación
- Diferentes tipos de documento
- Tests de rendimiento y carga

## Ejemplos de Uso

### Crear Beneficiario
```http
POST /api/beneficiarios
Content-Type: application/json

{
  "nombres": "Juan Carlos",
  "apellidos": "Pérez García",
  "documentoIdentidadId": 1,
  "numeroDocumento": "12345678",
  "fechaNacimiento": "1990-05-15"
}
```

### Error de Validación
```http
HTTP/1.1 400 Bad Request
Content-Type: application/json

{
  "errors": {
    "numeroDocumento": [
      "El documento DNI debe contener exactamente 8 dígitos numéricos"
    ]
  }
}
```

