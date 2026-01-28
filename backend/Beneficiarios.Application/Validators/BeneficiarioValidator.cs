using Beneficiarios.Application.DTOs;
using FluentValidation;

namespace Beneficiarios.Application.Validators;

public class CreateBeneficiarioValidator : AbstractValidator<CreateBeneficiarioDto>
{
    public CreateBeneficiarioValidator()
    {
        RuleFor(x => x.Nombres)
            .NotEmpty().WithMessage("Los nombres son requeridos")
            .MaximumLength(100).WithMessage("Los nombres no pueden exceder 100 caracteres");

        RuleFor(x => x.Apellidos)
            .NotEmpty().WithMessage("Los apellidos son requeridos")
            .MaximumLength(100).WithMessage("Los apellidos no pueden exceder 100 caracteres");

        RuleFor(x => x.DocumentoIdentidadId)
            .GreaterThan(0).WithMessage("Debe seleccionar un tipo de documento válido");

        RuleFor(x => x.NumeroDocumento)
            .NotEmpty().WithMessage("El número de documento es requerido");

        RuleFor(x => x.FechaNacimiento)
            .LessThan(DateTime.Today.AddYears(-18)).WithMessage("El beneficiario debe ser mayor de edad")
            .GreaterThan(DateTime.Today.AddYears(-120)).WithMessage("Fecha de nacimiento inválida");
    }
}

public class UpdateBeneficiarioValidator : AbstractValidator<UpdateBeneficiarioDto>
{
    public UpdateBeneficiarioValidator()
    {
        RuleFor(x => x.Nombres)
            .NotEmpty().WithMessage("Los nombres son requeridos")
            .MaximumLength(100).WithMessage("Los nombres no pueden exceder 100 caracteres");

        RuleFor(x => x.Apellidos)
            .NotEmpty().WithMessage("Los apellidos son requeridos")
            .MaximumLength(100).WithMessage("Los apellidos no pueden exceder 100 caracteres");

        RuleFor(x => x.DocumentoIdentidadId)
            .GreaterThan(0).WithMessage("Debe seleccionar un tipo de documento válido");

        RuleFor(x => x.NumeroDocumento)
            .NotEmpty().WithMessage("El número de documento es requerido");

        RuleFor(x => x.FechaNacimiento)
            .LessThan(DateTime.Today.AddYears(-18)).WithMessage("El beneficiario debe ser mayor de edad")
            .GreaterThan(DateTime.Today.AddYears(-120)).WithMessage("Fecha de nacimiento inválida");
    }
}