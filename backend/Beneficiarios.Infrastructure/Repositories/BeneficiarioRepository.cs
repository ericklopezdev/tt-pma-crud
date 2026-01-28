using Beneficiarios.Application.Interfaces;
using Beneficiarios.Domain.Entities;
using Beneficiarios.Domain.Enums;
using Beneficiarios.Infrastructure.Db;
using Microsoft.Data.SqlClient;

namespace Beneficiarios.Infrastructure.Repositories;

public class BeneficiarioRepository : IRepository<Beneficiario>
{
    private readonly SqlConnectionFactory _connectionFactory;

    public BeneficiarioRepository(SqlConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<IEnumerable<Beneficiario>> GetAllAsync()
    {
        using var connection = _connectionFactory.CreateConnection();
        using var command = new SqlCommand("SP_BENEFICIARIOS_GET_ALL", connection as SqlConnection)
        {
            CommandType = System.Data.CommandType.StoredProcedure
        };

        using var reader = await command.ExecuteReaderAsync();
        var beneficiarios = new List<Beneficiario>();

        while (await reader.ReadAsync())
        {
            // Dentro de GetAllAsync y GetByIdAsync, corrige los bloques de asignación:

            var beneficiario = new Beneficiario
            {
                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                Nombres = reader.GetString(reader.GetOrdinal("Nombres")),
                Apellidos = reader.GetString(reader.GetOrdinal("Apellidos")),
                DocumentoIdentidadId = reader.GetInt32(reader.GetOrdinal("DocumentoIdentidadId")),
                NumeroDocumento = reader.GetString(reader.GetOrdinal("NumeroDocumento")),
                FechaNacimiento = reader.GetDateTime(reader.GetOrdinal("FechaNacimiento")),
                Estado = (EstadoEnum)reader.GetInt32(reader.GetOrdinal("Estado"))
            };

            if (!await reader.IsDBNullAsync(reader.GetOrdinal("DocumentoNombre")))
            {
                beneficiario.DocumentoIdentidad = new DocumentoIdentidad
                {
                    Id = reader.GetInt32(reader.GetOrdinal("DocumentoId")),
                    Nombre = reader.GetString(reader.GetOrdinal("DocumentoNombre")),
                    Codigo = reader.GetString(reader.GetOrdinal("DocumentoCodigo")),
                    Longitud = reader.GetInt32(reader.GetOrdinal("DocumentoLongitud")),
                    SoloNumeros = reader.GetBoolean(reader.GetOrdinal("DocumentoSoloNumeros")),
                    Estado = (EstadoEnum)reader.GetInt32(reader.GetOrdinal("DocumentoEstado"))
                };
            }
            beneficiarios.Add(beneficiario);
        }

        return beneficiarios;
    }

    public async Task<Beneficiario?> GetByIdAsync(int id)
    {
        using var connection = _connectionFactory.CreateConnection();
        using var command = new SqlCommand("SP_BENEFICIARIOS_GET_BY_ID", connection as SqlConnection)
        {
            CommandType = System.Data.CommandType.StoredProcedure
        };

        command.Parameters.AddWithValue("@Id", id);

        using var reader = await command.ExecuteReaderAsync();
        if (await reader.ReadAsync())
        {
            var beneficiario = new Beneficiario
            {
                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                Nombres = reader.GetString(reader.GetOrdinal("Nombres")),
                Apellidos = reader.GetString(reader.GetOrdinal("Apellidos")),
                DocumentoIdentidadId = reader.GetInt32(reader.GetOrdinal("DocumentoIdentidadId")),
                NumeroDocumento = reader.GetString(reader.GetOrdinal("NumeroDocumento")),
                FechaNacimiento = reader.GetDateTime(reader.GetOrdinal("FechaNacimiento")),
                Estado = (EstadoEnum)reader.GetInt32(reader.GetOrdinal("Estado"))
            };

            if (!await reader.IsDBNullAsync(reader.GetOrdinal("DocumentoNombre")))
            {
                beneficiario.DocumentoIdentidad = new DocumentoIdentidad
                {
                    Id = reader.GetInt32(reader.GetOrdinal("DocumentoId")),
                    Nombre = reader.GetString(reader.GetOrdinal("DocumentoNombre")),
                    Codigo = reader.GetString(reader.GetOrdinal("DocumentoCodigo")),
                    Longitud = reader.GetInt32(reader.GetOrdinal("DocumentoLongitud")),
                    SoloNumeros = reader.GetBoolean(reader.GetOrdinal("DocumentoSoloNumeros")),
                    Estado = (EstadoEnum)reader.GetInt32(reader.GetOrdinal("DocumentoEstado"))
                };
            }

            return beneficiario;
        }

        return null;
    }

    public async Task<Beneficiario> CreateAsync(Beneficiario entity)
    {
        using var connection = _connectionFactory.CreateConnection();
        using var command = new SqlCommand("SP_BENEFICIARIOS_INSERT", connection as SqlConnection)
        {
            CommandType = System.Data.CommandType.StoredProcedure
        };

        command.Parameters.AddWithValue("@Nombres", entity.Nombres);
        command.Parameters.AddWithValue("@Apellidos", entity.Apellidos);
        command.Parameters.AddWithValue("@DocumentoIdentidadId", entity.DocumentoIdentidadId);
        command.Parameters.AddWithValue("@NumeroDocumento", entity.NumeroDocumento);
        command.Parameters.AddWithValue("@FechaNacimiento", entity.FechaNacimiento);

        var idParameter = new SqlParameter("@Id", System.Data.SqlDbType.Int)
        {
            Direction = System.Data.ParameterDirection.Output
        };
        command.Parameters.Add(idParameter);

        await command.ExecuteNonQueryAsync();

        entity.Id = (int)idParameter.Value!;
        entity.Estado = EstadoEnum.Activo;

        return entity;
    }

    public async Task<bool> UpdateAsync(Beneficiario entity)
    {
        using var connection = _connectionFactory.CreateConnection();
        using var command = new SqlCommand("SP_BENEFICIARIOS_UPDATE", connection as SqlConnection)
        {
            CommandType = System.Data.CommandType.StoredProcedure
        };

        command.Parameters.AddWithValue("@Id", entity.Id);
        command.Parameters.AddWithValue("@Nombres", entity.Nombres);
        command.Parameters.AddWithValue("@Apellidos", entity.Apellidos);
        command.Parameters.AddWithValue("@DocumentoIdentidadId", entity.DocumentoIdentidadId);
        command.Parameters.AddWithValue("@NumeroDocumento", entity.NumeroDocumento);
        command.Parameters.AddWithValue("@FechaNacimiento", entity.FechaNacimiento);
        command.Parameters.AddWithValue("@Estado", (int)entity.Estado);

        var rowsAffected = await command.ExecuteNonQueryAsync();
        return rowsAffected > 0;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        using var connection = _connectionFactory.CreateConnection();
        using var command = new SqlCommand("SP_BENEFICIARIOS_DELETE", connection as SqlConnection)
        {
            CommandType = System.Data.CommandType.StoredProcedure
        };

        command.Parameters.AddWithValue("@Id", id);

        var rowsAffected = await command.ExecuteNonQueryAsync();
        return rowsAffected > 0;
    }
}
