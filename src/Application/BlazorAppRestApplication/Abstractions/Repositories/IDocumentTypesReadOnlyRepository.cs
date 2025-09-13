using Domain.Documentos;

namespace Application.Abstractions.Repositories;

public interface IDocumentTypesReadOnlyRepository
{
    Task<IReadOnlyList<DocumentType>> GetAllAsync(CancellationToken ct = default);
}
