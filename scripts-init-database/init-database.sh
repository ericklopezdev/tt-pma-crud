#!/bin/bash

set -e

echo "🚀 Initializing Beneficiarios Database..."

# Configuration
SQLSERVER_CONTAINER="sqlserver"
SA_PASSWORD="YourStrong!Password"
DATABASE_NAME="BeneficiariosDB"
SCRIPTS_PATH="/docker-entrypoint-initdb.d"

# Colors for output
RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
BLUE='\033[0;34m'
NC='\033[0m' # No Color

# Function to print colored output
print_status() {
    echo -e "${BLUE}[INFO]${NC} $1"
}

print_success() {
    echo -e "${GREEN}[SUCCESS]${NC} $1"
}

print_warning() {
    echo -e "${YELLOW}[WARNING]${NC} $1"
}

print_error() {
    echo -e "${RED}[ERROR]${NC} $1"
}

# Check if Docker is running
if ! docker info > /dev/null 2>&1; then
    print_error "Docker is not running. Please start Docker first."
    exit 1
fi

# Check if SQL Server container is running
if ! docker ps --format "table {{.Names}}" | grep -q "^${SQLSERVER_CONTAINER}$"; then
    print_error "SQL Server container '${SQLSERVER_CONTAINER}' is not running."
    print_status "Please start the container with: docker-compose up sqlserver -d"
    exit 1
fi

print_status "SQL Server container is running ✅"

# Wait for SQL Server to be ready
print_status "Waiting for SQL Server to be ready..."
max_attempts=30
attempt=1

while [ $attempt -le $max_attempts ]; do
    if docker exec ${SQLSERVER_CONTAINER} /opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P "${SA_PASSWORD}" -C -Q "SELECT 1" > /dev/null 2>&1; then
        print_success "SQL Server is ready! 🎉"
        break
    fi
    
    if [ $attempt -eq $max_attempts ]; then
        print_error "SQL Server failed to become ready after ${max_attempts} attempts."
        exit 1
    fi
    
    print_status "Attempt ${attempt}/${max_attempts}: SQL Server not ready, waiting 5 seconds..."
    sleep 5
    ((attempt++))
done

# Check if database already exists
print_status "Checking if database '${DATABASE_NAME}' exists..."
DB_EXISTS=$(docker exec ${SQLSERVER_CONTAINER} /opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P "${SA_PASSWORD}" -C -Q "SELECT COUNT(*) FROM sys.databases WHERE name = '${DATABASE_NAME}'" -h -1 | tr -d '[:space:]')

if [ "$DB_EXISTS" = "1" ]; then
    print_warning "Database '${DATABASE_NAME}' already exists."
    read -p "Do you want to drop and recreate it? (y/N): " -n 1 -r
    echo
    if [[ $REPLY =~ ^[Yy]$ ]]; then
        print_status "Dropping existing database..."
        docker exec ${SQLSERVER_CONTAINER} /opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P "${SA_PASSWORD}" -C -Q "DROP DATABASE ${DATABASE_NAME}"
        print_success "Database dropped successfully."
    else
        print_status "Skipping database creation."
        exit 0
    fi
fi

# Create the database
print_status "Creating database '${DATABASE_NAME}'..."
if docker exec ${SQLSERVER_CONTAINER} /opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P "${SA_PASSWORD}" -C -Q "CREATE DATABASE ${DATABASE_NAME}"; then
    print_success "Database '${DATABASE_NAME}' created successfully! 🎉"
else
    print_error "Failed to create database '${DATABASE_NAME}'."
    exit 1
fi

# Check if scripts exist in the container
print_status "Checking for initialization scripts in container..."
SCRIPTS_COUNT=$(docker exec ${SQLSERVER_CONTAINER} ls ${SCRIPTS_PATH} 2>/dev/null | wc -l)

if [ "$SCRIPTS_COUNT" -eq 0 ]; then
    print_error "No initialization scripts found in container at ${SCRIPTS_PATH}"
    print_error "Please ensure the volume mount is correctly configured in docker-compose.yml"
    exit 1
fi

print_success "Found ${SCRIPTS_COUNT} initialization scripts."

# Run initialization scripts in order
print_status "Running initialization scripts..."

for script in 01_create_tables.sql 02_insert_documentos.sql 03_sp_documentos.sql 04_sp_beneficiarios.sql; do
    print_status "Executing ${script}..."
    if docker exec ${SQLSERVER_CONTAINER} /opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P "${SA_PASSWORD}" -C -d "${DATABASE_NAME}" -i "${SCRIPTS_PATH}/${script}"; then
        print_success "✅ ${script} executed successfully."
    else
        print_error "❌ Failed to execute ${script}."
        exit 1
    fi
done

# Verify database setup
print_status "Verifying database setup..."

# Check if tables were created
TABLE_COUNT=$(docker exec ${SQLSERVER_CONTAINER} /opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P "${SA_PASSWORD}" -C -d "${DATABASE_NAME}" -Q "SELECT COUNT(*) FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE = 'BASE TABLE'" -h -1 | tr -d '[:space:]')
print_success "Database contains ${TABLE_COUNT} tables."

# Check if stored procedures were created
SP_COUNT=$(docker exec ${SQLSERVER_CONTAINER} /opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P "${SA_PASSWORD}" -C -d "${DATABASE_NAME}" -Q "SELECT COUNT(*) FROM INFORMATION_SCHEMA.ROUTINES WHERE ROUTINE_TYPE = 'PROCEDURE'" -h -1 | tr -d '[:space:]')
print_success "Database contains ${SP_COUNT} stored procedures."

print_success "🎉 Database initialization completed successfully!"
print_status "Database: ${DATABASE_NAME}"
print_status "Container: ${SQLSERVER_CONTAINER}"
print_status "Connection string: Server=sqlserver,1433;Database=${DATABASE_NAME};User Id=sa;Password=${SA_PASSWORD};TrustServerCertificate=true;"

echo ""
print_status "You can now restart your backend container to test the connection:"
echo "  docker-compose restart backend"