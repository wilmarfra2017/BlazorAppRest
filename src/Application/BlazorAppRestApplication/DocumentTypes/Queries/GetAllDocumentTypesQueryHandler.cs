using Application.Abstractions.Repositories;
using Domain.Documentos;
using MediatR;

namespace Application.DocumentTypes.Queries;

internal sealed class GetAllDocumentTypesQueryHandler
    : IRequestHandler<GetAllDocumentTypesQuery, IReadOnlyList<DocumentTypeDto>>
{
    private readonly IDocumentTypesReadOnlyRepository _repo;

    public GetAllDocumentTypesQueryHandler(IDocumentTypesReadOnlyRepository repo)
        => _repo = repo;

    public async Task<IReadOnlyList<DocumentTypeDto>> Handle(
        GetAllDocumentTypesQuery request, CancellationToken ct)
    {
        var items = await _repo.GetAllAsync(ct);
        
        return items
            .Select(x => new DocumentTypeDto(x.Codigo, x.Descripcion, x.Activa))
            .OrderBy(x => x.Codigo)
            .ToList();
    }

    private static DocumentTypeDto Map(DocumentType x)
        => new DocumentTypeDto(x.Codigo, x.Descripcion, x.Activa);
}
