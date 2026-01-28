using Beneficiarios.Application.DTOs;
using Beneficiarios.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Beneficiarios.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DocumentosIdentidadController : ControllerBase
{
    private readonly IDocumentoIdentidadService _documentoService;

    public DocumentosIdentidadController(IDocumentoIdentidadService documentoService)
    {
        _documentoService = documentoService;
    }

    /// <summary>
    /// Obtiene todos los tipos de documento de identidad activos
    /// </summary>
    /// <returns>Lista de documentos de identidad activos</returns>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<DocumentoIdentidadDto>>> GetActivos()
    {
        try
        {
            var documentos = await _documentoService.GetActivosAsync();
            return Ok(documentos);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error interno del servidor: {ex.Message}");
        }
    }
}
