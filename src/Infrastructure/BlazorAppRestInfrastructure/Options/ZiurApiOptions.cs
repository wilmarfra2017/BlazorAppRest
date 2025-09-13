using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Options;

public sealed class ZiurApiOptions
{
    public const string SectionName = "ZiurApi";
    
    [Required, Url]
    public string BaseUrl { get; init; } = default!;

    [Required]
    public string DocumentTypesPath { get; init; } = default!;

    [Required]
    public string Token { get; init; } = default!;
}
