using System.Net.Http.Json;
using System.Text.Json;
using Infrastructure.Models;
using Infrastructure.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Infrastructure.Http;

internal interface IZiurApiClient
{
    Task<IReadOnlyList<ZiurDocumentTypeDto>> GetDocumentTypesAsync(CancellationToken ct);
}

internal sealed class ZiurApiClient : IZiurApiClient
{
    private readonly HttpClient _http;
    private readonly ZiurApiOptions _opts;
    private readonly ILogger<ZiurApiClient> _logger;

    public ZiurApiClient(HttpClient http, IOptions<ZiurApiOptions> opts, ILogger<ZiurApiClient> logger)
    {
        _http = http;
        _opts = opts.Value;
        _logger = logger;
    }

    public async Task<IReadOnlyList<ZiurDocumentTypeDto>> GetDocumentTypesAsync(CancellationToken ct)
    {
        _logger.LogInformation("Calling Ziur API {Path}", _opts.DocumentTypesPath);

        var response = await _http.GetAsync(_opts.DocumentTypesPath, ct);
        response.EnsureSuccessStatusCode();

        var items = await response.Content.ReadFromJsonAsync<List<ZiurDocumentTypeDto>>(
        options: new JsonSerializerOptions(JsonSerializerDefaults.Web)
        {
            PropertyNameCaseInsensitive = true
        },
        cancellationToken: ct);

        return items ?? new List<ZiurDocumentTypeDto>();
    }
}
