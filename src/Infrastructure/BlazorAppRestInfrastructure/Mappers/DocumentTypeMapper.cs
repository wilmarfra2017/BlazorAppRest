using Domain.Documentos;
using Infrastructure.Models;

namespace Infrastructure.Mappers;

internal static class DocumentTypeMapper
{
    public static DocumentType ToDomain(this ZiurDocumentTypeDto dto)
        => new DocumentType(dto.Codigo, dto.Descripcion, dto.VActiva);
}
