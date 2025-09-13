// Program.cs (Blazor Web App .NET 8)
using Application.DocumentTypes.Queries;
using Infrastructure;
using MediatR;
using BlazorAppRest.Components;
using Microsoft.AspNetCore.Components;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddHttpClient();

builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(GetAllDocumentTypesQuery).Assembly));

builder.Services.AddScoped(sp =>
{
    var nav = sp.GetRequiredService<NavigationManager>();
    return new HttpClient { BaseAddress = new Uri(nav.BaseUri) };
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
   .AddInteractiveServerRenderMode();


app.MapGet("/api/document-types",
    async (IMediator mediator, CancellationToken ct) =>
    {
        var data = await mediator.Send(new GetAllDocumentTypesQuery(), ct);
        return Results.Ok(data);
    })
    .WithName("GetDocumentos");


app.Run();
