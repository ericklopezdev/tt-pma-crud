using Beneficiarios.Application.DTOs;

namespace Beneficiarios.Application.Interfaces;

public interface IDocumentoIdentidadService
{
    Task<IEnumerable<DocumentoIdentidadDto>> GetActivosAsync();
}