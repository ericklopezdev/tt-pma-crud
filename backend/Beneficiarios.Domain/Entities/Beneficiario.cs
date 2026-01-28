using Beneficiarios.Domain.Enums;

namespace Beneficiarios.Domain.Entities;

public class Beneficiario
{
    public int Id { get; set; }
    public string Nombres { get; set; } = string.Empty;
    public string Apellidos { get; set; } = string.Empty;
    public int DocumentoIdentidadId { get; set; }
    public string NumeroDocumento { get; set; } = string.Empty;
    public DateTime FechaNacimiento { get; set; }
    public EstadoEnum Estado { get; set; }
    
    // Propiedad de navegación
    public DocumentoIdentidad? DocumentoIdentidad { get; set; }
}