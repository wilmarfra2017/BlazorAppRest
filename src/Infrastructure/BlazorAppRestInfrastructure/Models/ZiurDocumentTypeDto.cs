namespace Infrastructure.Models;

internal sealed class ZiurDocumentTypeDto
{
    public int Codigo { get; init; }
    public string Descripcion { get; init; } = string.Empty;
    public bool VActiva { get; init; }
}
