using Beneficiarios.Domain.Enums;

namespace Beneficiarios.Domain.Entities;

public class DocumentoIdentidad
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Codigo { get; set; } = string.Empty;
    public int Longitud { get; set; }
    public bool SoloNumeros { get; set; }
    public EstadoEnum Estado { get; set; }
}