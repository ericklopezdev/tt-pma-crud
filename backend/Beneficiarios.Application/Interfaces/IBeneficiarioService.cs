using Beneficiarios.Application.DTOs;

namespace Beneficiarios.Application.Interfaces;

public interface IBeneficiarioService
{
    Task<IEnumerable<BeneficiarioDto>> GetAllAsync();
    Task<BeneficiarioDto?> GetByIdAsync(int id);
    Task<BeneficiarioDto> CreateAsync(CreateBeneficiarioDto dto);
    Task<bool> UpdateAsync(int id, UpdateBeneficiarioDto dto);
    Task<bool> DeleteAsync(int id);
}