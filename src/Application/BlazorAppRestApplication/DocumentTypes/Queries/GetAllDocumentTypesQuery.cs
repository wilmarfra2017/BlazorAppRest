using MediatR;

namespace Application.DocumentTypes.Queries;

public sealed record GetAllDocumentTypesQuery : IRequest<IReadOnlyList<DocumentTypeDto>>;

public sealed record DocumentTypeDto(int Codigo, string Descripcion, bool Activa);
