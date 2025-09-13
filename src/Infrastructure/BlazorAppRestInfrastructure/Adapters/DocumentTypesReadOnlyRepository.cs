using Application.Abstractions.Repositories;
using Domain.Documentos;
using Infrastructure.Http;
using Infrastructure.Mappers;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Adapters;

internal sealed class DocumentTypesReadOnlyRepository : IDocumentTypesReadOnlyRepository
{
    private readonly IZiurApiClient _client;
    private readonly ILogger<DocumentTypesReadOnlyRepository> _logger;

    public DocumentTypesReadOnlyRepository(IZiurApiClient client, ILogger<DocumentTypesReadOnlyRepository> logger)
    {
        _client = client;
        _logger = logger;
    }

    public async Task<IReadOnlyList<DocumentType>> GetAllAsync(CancellationToken ct = default)
    {
        _logger.LogDebug("Obteniendo tipos de documentos de la API externa de Ziur...");
        var dtos = await _client.GetDocumentTypesAsync(ct);
        return dtos.Select(x => x.ToDomain()).ToList();
    }
}
