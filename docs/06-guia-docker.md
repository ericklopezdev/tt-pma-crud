# Guía Docker

## Puntos de Acceso

- **Frontend**: http://localhost:3000
- **Backend API**: http://localhost:5000  
- **Swagger**: http://localhost:5000/swagger

## 📋 Requisitos Previos
- Docker Desktop (Windows) o Docker Engine (Linux/Mac)
- Docker Compose v2.0+
- Git para clonar el repositorio

## 🏗️ Servicios Iniciados

| Servicio | Puerto | Descripción |
|----------|--------|-------------|
| SQL Server | 1433 | Base de datos con persistencia |
| Backend .NET | 5000 | API REST con clean architecture |
| Frontend React | 3000 | SPA con TailwindCSS |

## 🔧 Comandos Útiles

### Desarrollo
```bash
# Iniciar en background
docker compose up --build -d

# Ver logs
docker compose logs -f backend
docker compose logs -f frontend

# Reiniciar servicios
docker compose restart
```

### Mantenimiento
```bash
# Detener todo
docker compose down

# Limpiar todo (incluye datos)
docker compose down -v

# Reconstruir sin caché
docker compose build --no-cache
```

## 🗄️ Base de Datos

**Configuración automática**:
- Crea `BeneficiariosDB` automáticamente
- Ejecuta scripts de inicialización en orden:
  - `01_create_tables.sql`
  - `02_insert_documentos.sql` 
  - `03_sp_documentos.sql`
  - `04_sp_beneficiarios.sql`

**Acceso directo**:
```bash
# Conectar a SQL Server
docker exec -it sqlserver bash
sqlcmd -S localhost -U sa -P "YourStrong!Password" -d BeneficiariosDB
```

## ❌ Troubleshooting

### SQL Server no inicia
```bash
# Verificar estado
docker ps
docker logs sqlserver

# Esperar y verificar conexión
docker exec sqlserver /opt/mssql-tools18/bin/sqlcmd \
  -S localhost -U sa -P "YourStrong!Password" -C -Q "SELECT 1"
```

### Conflictos de puertos
```bash
# Ver qué usa los puertos
netstat -tulpn | grep :3000  # Frontend
netstat -tulpn | grep :5000  # Backend  
netstat -tulpn | grep :1433  # SQL Server
```

### Docker lento
```bash
# Limpiar sistema
docker system prune -a
docker compose down -v --rmi all
docker compose up --build
```

