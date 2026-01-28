using Beneficiarios.Application.Interfaces;
using Beneficiarios.Domain.Entities;
using Beneficiarios.Domain.Enums;
using Beneficiarios.Infrastructure.Db;
using Microsoft.Data.SqlClient;

namespace Beneficiarios.Infrastructure.Repositories;

public class DocumentoIdentidadRepository : IRepository<DocumentoIdentidad>
{
    private readonly SqlConnectionFactory _connectionFactory;

    public DocumentoIdentidadRepository(SqlConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<IEnumerable<DocumentoIdentidad>> GetAllAsync()
    {
        using var connection = _connectionFactory.CreateConnection();
        using var command = new SqlCommand("SP_DOCUMENTOS_IDENTIDAD_GET_ALL", connection as SqlConnection)
        {
            CommandType = System.Data.CommandType.StoredProcedure
        };

        using var reader = await command.ExecuteReaderAsync();
        var documentos = new List<DocumentoIdentidad>();

        while (await reader.ReadAsync())
        {
            documentos.Add(new DocumentoIdentidad
            {
                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                Nombre = reader.GetString(reader.GetOrdinal("Nombre")),
                Codigo = reader.GetString(reader.GetOrdinal("Codigo")),
                Longitud = reader.GetInt32(reader.GetOrdinal("Longitud")),
                SoloNumeros = reader.GetBoolean(reader.GetOrdinal("SoloNumeros")),
                Estado = (EstadoEnum)reader.GetInt32(reader.GetOrdinal("Estado"))
            });
        }

        return documentos;
    }

    public async Task<DocumentoIdentidad?> GetByIdAsync(int id)
    {
        using var connection = _connectionFactory.CreateConnection();
        using var command = new SqlCommand("SP_DOCUMENTOS_IDENTIDAD_GET_BY_ID", connection as SqlConnection)
        {
            CommandType = System.Data.CommandType.StoredProcedure
        };

        command.Parameters.AddWithValue("@Id", id);

        using var reader = await command.ExecuteReaderAsync();
        if (await reader.ReadAsync())
        {
            return new DocumentoIdentidad
            {
                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                Nombre = reader.GetString(reader.GetOrdinal("Nombre")),
                Codigo = reader.GetString(reader.GetOrdinal("Codigo")),
                Longitud = reader.GetInt32(reader.GetOrdinal("Longitud")),
                SoloNumeros = reader.GetBoolean(reader.GetOrdinal("SoloNumeros")),
                Estado = (EstadoEnum)reader.GetInt32(reader.GetOrdinal("Estado"))
            };
        }

        return null;
    }

    public async Task<DocumentoIdentidad> CreateAsync(DocumentoIdentidad entity)
    {
        using var connection = _connectionFactory.CreateConnection();
        using var command = new SqlCommand("SP_DOCUMENTOS_IDENTIDAD_INSERT", connection as SqlConnection)
        {
            CommandType = System.Data.CommandType.StoredProcedure
        };

        command.Parameters.AddWithValue("@Nombre", entity.Nombre);
        command.Parameters.AddWithValue("@Codigo", entity.Codigo);
        command.Parameters.AddWithValue("@Longitud", entity.Longitud);
        command.Parameters.AddWithValue("@SoloNumeros", entity.SoloNumeros);

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

    public async Task<bool> UpdateAsync(DocumentoIdentidad entity)
    {
        using var connection = _connectionFactory.CreateConnection();
        using var command = new SqlCommand("SP_DOCUMENTOS_IDENTIDAD_UPDATE", connection as SqlConnection)
        {
            CommandType = System.Data.CommandType.StoredProcedure
        };

        command.Parameters.AddWithValue("@Id", entity.Id);
        command.Parameters.AddWithValue("@Nombre", entity.Nombre);
        command.Parameters.AddWithValue("@Codigo", entity.Codigo);
        command.Parameters.AddWithValue("@Longitud", entity.Longitud);
        command.Parameters.AddWithValue("@SoloNumeros", entity.SoloNumeros);
        command.Parameters.AddWithValue("@Estado", (int)entity.Estado);

        var rowsAffected = await command.ExecuteNonQueryAsync();
        return rowsAffected > 0;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        using var connection = _connectionFactory.CreateConnection();
        using var command = new SqlCommand("SP_DOCUMENTOS_IDENTIDAD_DELETE", connection as SqlConnection)
        {
            CommandType = System.Data.CommandType.StoredProcedure
        };

        command.Parameters.AddWithValue("@Id", id);

        var rowsAffected = await command.ExecuteNonQueryAsync();
        return rowsAffected > 0;
    }
}