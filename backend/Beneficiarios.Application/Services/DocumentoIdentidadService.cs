using Beneficiarios.Application.DTOs;
using Beneficiarios.Application.Interfaces;
using Beneficiarios.Domain.Entities;
using Beneficiarios.Domain.Enums;

namespace Beneficiarios.Application.Services;

public class DocumentoIdentidadService : IDocumentoIdentidadService
{
    private readonly IRepository<DocumentoIdentidad> _repository;

    public DocumentoIdentidadService(IRepository<DocumentoIdentidad> repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<DocumentoIdentidadDto>> GetActivosAsync()
    {
        var documentos = await _repository.GetAllAsync();
        return documentos
            .Where(d => d.Estado == EstadoEnum.Activo)
            .Select(d => new DocumentoIdentidadDto
            {
                Id = d.Id,
                Nombre = d.Nombre,
                Codigo = d.Codigo,
                Longitud = d.Longitud,
                SoloNumeros = d.SoloNumeros,
                Estado = d.Estado
            });
    }
}