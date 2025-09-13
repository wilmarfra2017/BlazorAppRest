using Application.Abstractions.Repositories;
using Infrastructure.Http;
using Infrastructure.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Polly;
using Polly.Extensions.Http;
using System.Net.Http.Headers;


namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {        
        services.AddOptions<ZiurApiOptions>()
            .Bind(configuration.GetSection(ZiurApiOptions.SectionName))
            .ValidateDataAnnotations()
            .Validate(o => !string.IsNullOrWhiteSpace(o.Token), "Token is required")
            .ValidateOnStart();
        
        var retryPolicy = HttpPolicyExtensions
            .HandleTransientHttpError()
            .OrResult(r => (int)r.StatusCode == 429)
            .WaitAndRetryAsync(new[]
            {
                TimeSpan.FromMilliseconds(200),
                TimeSpan.FromMilliseconds(600),
                TimeSpan.FromSeconds(1.2)
            });

        var timeoutPolicy = Policy.TimeoutAsync<HttpResponseMessage>(TimeSpan.FromSeconds(10));
        var circuitPolicy = HttpPolicyExtensions
            .HandleTransientHttpError()
            .CircuitBreakerAsync(handledEventsAllowedBeforeBreaking: 5, durationOfBreak: TimeSpan.FromSeconds(30));

        services.AddHttpClient<IZiurApiClient, ZiurApiClient>((sp, http) =>
        {
            var opts = sp.GetRequiredService<IOptions<ZiurApiOptions>>().Value;

            http.BaseAddress = new Uri(opts.BaseUrl);
            http.DefaultRequestHeaders.Accept.Clear();
            http.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", opts.Token);
        })
        .AddPolicyHandler(retryPolicy)
        .AddPolicyHandler(timeoutPolicy)
        .AddPolicyHandler(circuitPolicy);

        services.AddScoped<IDocumentTypesReadOnlyRepository,
                    Infrastructure.Adapters.DocumentTypesReadOnlyRepository>();


        return services;
    }
}
