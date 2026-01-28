using Beneficiarios.Application.DTOs;
using Beneficiarios.Application.Interfaces;
using Beneficiarios.Domain.Entities;
using Beneficiarios.Domain.Enums;
using FluentValidation;

namespace Beneficiarios.Application.Services;

public class BeneficiarioService : IBeneficiarioService
{
    private readonly IRepository<Beneficiario> _beneficiarioRepository;
    private readonly IRepository<DocumentoIdentidad> _documentoRepository;
    private readonly IValidator<CreateBeneficiarioDto> _createValidator;
    private readonly IValidator<UpdateBeneficiarioDto> _updateValidator;

    public BeneficiarioService(
        IRepository<Beneficiario> beneficiarioRepository,
        IRepository<DocumentoIdentidad> documentoRepository,
        IValidator<CreateBeneficiarioDto> createValidator,
        IValidator<UpdateBeneficiarioDto> updateValidator)
    {
        _beneficiarioRepository = beneficiarioRepository;
        _documentoRepository = documentoRepository;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
    }

    public async Task<IEnumerable<BeneficiarioDto>> GetAllAsync()
    {
        var beneficiarios = await _beneficiarioRepository.GetAllAsync();
        return beneficiarios.Select(b => new BeneficiarioDto
        {
            Id = b.Id,
            Nombres = b.Nombres,
            Apellidos = b.Apellidos,
            DocumentoIdentidadId = b.DocumentoIdentidadId,
            NumeroDocumento = b.NumeroDocumento,
            FechaNacimiento = b.FechaNacimiento,
            Estado = b.Estado,
            DocumentoIdentidad = b.DocumentoIdentidad != null ? new DocumentoIdentidadDto
            {
                Id = b.DocumentoIdentidad.Id,
                Nombre = b.DocumentoIdentidad.Nombre,
                Codigo = b.DocumentoIdentidad.Codigo,
                Longitud = b.DocumentoIdentidad.Longitud,
                SoloNumeros = b.DocumentoIdentidad.SoloNumeros,
                Estado = b.DocumentoIdentidad.Estado
            } : null
        });
    }

    public async Task<BeneficiarioDto?> GetByIdAsync(int id)
    {
        var beneficiario = await _beneficiarioRepository.GetByIdAsync(id);
        if (beneficiario == null) return null;

        return new BeneficiarioDto
        {
            Id = beneficiario.Id,
            Nombres = beneficiario.Nombres,
            Apellidos = beneficiario.Apellidos,
            DocumentoIdentidadId = beneficiario.DocumentoIdentidadId,
            NumeroDocumento = beneficiario.NumeroDocumento,
            FechaNacimiento = beneficiario.FechaNacimiento,
            Estado = beneficiario.Estado,
            DocumentoIdentidad = beneficiario.DocumentoIdentidad != null ? new DocumentoIdentidadDto
            {
                Id = beneficiario.DocumentoIdentidad.Id,
                Nombre = beneficiario.DocumentoIdentidad.Nombre,
                Codigo = beneficiario.DocumentoIdentidad.Codigo,
                Longitud = beneficiario.DocumentoIdentidad.Longitud,
                SoloNumeros = beneficiario.DocumentoIdentidad.SoloNumeros,
                Estado = beneficiario.DocumentoIdentidad.Estado
            } : null
        };
    }

    public async Task<BeneficiarioDto> CreateAsync(CreateBeneficiarioDto dto)
    {
        // Validación básica
        var validationResult = await _createValidator.ValidateAsync(dto);
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }

        // Validación condicional basada en el tipo de documento
        var documento = await _documentoRepository.GetByIdAsync(dto.DocumentoIdentidadId);
        if (documento == null)
        {
            throw new Exception("Tipo de documento no encontrado");
        }

        // Validar longitud del número de documento
        if (dto.NumeroDocumento.Length != documento.Longitud)
        {
            throw new ValidationException(new List<FluentValidation.Results.ValidationFailure>
            {
                new FluentValidation.Results.ValidationFailure("NumeroDocumento", $"El número de documento debe tener {documento.Longitud} caracteres")
            });
        }

        // Validar si solo permite números
        if (documento.SoloNumeros && !dto.NumeroDocumento.All(char.IsDigit))
        {
            throw new ValidationException(new List<FluentValidation.Results.ValidationFailure>
            {
                new FluentValidation.Results.ValidationFailure("NumeroDocumento", "El número de documento solo debe contener números")
            });
        }

        var beneficiario = new Beneficiario
        {
            Nombres = dto.Nombres,
            Apellidos = dto.Apellidos,
            DocumentoIdentidadId = dto.DocumentoIdentidadId,
            NumeroDocumento = dto.NumeroDocumento,
            FechaNacimiento = dto.FechaNacimiento,
            Estado = EstadoEnum.Activo
        };

        var created = await _beneficiarioRepository.CreateAsync(beneficiario);

        return new BeneficiarioDto
        {
            Id = created.Id,
            Nombres = created.Nombres,
            Apellidos = created.Apellidos,
            DocumentoIdentidadId = created.DocumentoIdentidadId,
            NumeroDocumento = created.NumeroDocumento,
            FechaNacimiento = created.FechaNacimiento,
            Estado = created.Estado
        };
    }

    public async Task<bool> UpdateAsync(int id, UpdateBeneficiarioDto dto)
    {
        // Validación básica
        var validationResult = await _updateValidator.ValidateAsync(dto);
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }

        var existing = await _beneficiarioRepository.GetByIdAsync(id);
        if (existing == null) return false;

        // Validación condicional basada en el tipo de documento
        var documento = await _documentoRepository.GetByIdAsync(dto.DocumentoIdentidadId);
        if (documento == null)
        {
            throw new Exception("Tipo de documento no encontrado");
        }

        // Validar longitud del número de documento
        if (dto.NumeroDocumento.Length != documento.Longitud)
        {
            throw new ValidationException(new List<FluentValidation.Results.ValidationFailure>
            {
                new FluentValidation.Results.ValidationFailure("NumeroDocumento", $"El número de documento debe tener {documento.Longitud} caracteres")
            });
        }

        // Validar si solo permite números
        if (documento.SoloNumeros && !dto.NumeroDocumento.All(char.IsDigit))
        {
            throw new ValidationException(new List<FluentValidation.Results.ValidationFailure>
            {
                new FluentValidation.Results.ValidationFailure("NumeroDocumento", "El número de documento solo debe contener números")
            });
        }

        existing.Nombres = dto.Nombres;
        existing.Apellidos = dto.Apellidos;
        existing.DocumentoIdentidadId = dto.DocumentoIdentidadId;
        existing.NumeroDocumento = dto.NumeroDocumento;
        existing.FechaNacimiento = dto.FechaNacimiento;
        existing.Estado = dto.Estado;

        return await _beneficiarioRepository.UpdateAsync(existing);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var existing = await _beneficiarioRepository.GetByIdAsync(id);
        if (existing == null) return false;

        return await _beneficiarioRepository.DeleteAsync(id);
    }
}
