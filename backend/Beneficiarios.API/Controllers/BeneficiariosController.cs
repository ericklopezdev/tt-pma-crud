using Beneficiarios.Application.DTOs;
using Beneficiarios.Application.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace Beneficiarios.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BeneficiariosController : ControllerBase
{
    private readonly IBeneficiarioService _beneficiarioService;

    public BeneficiariosController(IBeneficiarioService beneficiarioService)
    {
        _beneficiarioService = beneficiarioService;
    }

    /// <summary>
    /// Obtiene todos los beneficiarios
    /// </summary>
    /// <returns>Lista de beneficiarios</returns>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<BeneficiarioDto>>> GetAll()
    {
        try
        {
            var beneficiarios = await _beneficiarioService.GetAllAsync();
            return Ok(beneficiarios);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error interno del servidor: {ex.Message}");
        }
    }

    /// <summary>
    /// Obtiene un beneficiario por su ID
    /// </summary>
    /// <param name="id">ID del beneficiario</param>
    /// <returns>Beneficiario encontrado</returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<BeneficiarioDto>> GetById(int id)
    {
        try
        {
            var beneficiario = await _beneficiarioService.GetByIdAsync(id);
            if (beneficiario == null)
            {
                return NotFound($"Beneficiario con ID {id} no encontrado");
            }
            return Ok(beneficiario);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error interno del servidor: {ex.Message}");
        }
    }

    /// <summary>
    /// Crea un nuevo beneficiario
    /// </summary>
    /// <param name="dto">Datos del nuevo beneficiario</param>
    /// <returns>Beneficiario creado</returns>
    [HttpPost]
    public async Task<ActionResult<BeneficiarioDto>> Create([FromBody] CreateBeneficiarioDto dto)
    {
        try
        {
            var beneficiario = await _beneficiarioService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = beneficiario.Id }, beneficiario);
        }
        catch (ValidationException ex)
        {
            return BadRequest(ex.Errors.Select(e => new
            {
                Property = e.PropertyName,
                Error = e.ErrorMessage
            }));
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error interno del servidor: {ex.Message}");
        }
    }

    /// <summary>
    /// Actualiza un beneficiario existente
    /// </summary>
    /// <param name="id">ID del beneficiario</param>
    /// <param name="dto">Datos actualizados del beneficiario</param>
    /// <returns>Resultado de la operación</returns>
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateBeneficiarioDto dto)
    {
        try
        {
            var result = await _beneficiarioService.UpdateAsync(id, dto);
            if (!result)
            {
                return NotFound($"Beneficiario con ID {id} no encontrado");
            }
            return NoContent();
        }
        catch (ValidationException ex)
        {
            return BadRequest(ex.Errors.Select(e => new
            {
                Property = e.PropertyName,
                Error = e.ErrorMessage
            }));
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error interno del servidor: {ex.Message}");
        }
    }

    /// <summary>
    /// Elimina (desactiva) un beneficiario
    /// </summary>
    /// <param name="id">ID del beneficiario</param>
    /// <returns>Resultado de la operación</returns>
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var result = await _beneficiarioService.DeleteAsync(id);
            if (!result)
            {
                return NotFound($"Beneficiario con ID {id} no encontrado");
            }
            return NoContent();
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error interno del servidor: {ex.Message}");
        }
    }
}
