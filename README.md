# BlazorAppRest

Aplicación Blazor Web App (.NET 8) que consume un endpoint externo de Ziur para listar tipos de documento en una grilla.
Sigue Arquitectura Hexagonal, patrón CQRS con MediatR, endpoints Minimal API internos, resiliencia con Polly y DI nativa.

## Tecnologías

- .NET 8 – Blazor Web App (Razor Components)

- CQRS con MediatR (v12)

- Arquitectura Hexagonal (Domain, Application, Infrastructure, Web)

- Minimal API (endpoint interno /api/document-types)

- HttpClient + Polly (retry/timeout/circuit-breaker)

- Options con validación por data annotations

- Scoped CSS para estilos de la grilla

- Endpoint externo (Ziur)

API:
https://mainserver.ziursoftware.com/Ziur.API/basedatos_01/ZiurServiceRest.svc/api/DocumentosFillsCombos

Auth: Bearer Token (configurado por IOptions<ZiurApiOptions>)

Ejemplo de respuesta:

[
  { "Codigo": 29, "Descripcion": "Ajuste al Inventario", "VActiva": false },
  { "Codigo": 51, "Descripcion": "Avance Produccion",  "VActiva": false },
  { "Codigo": 17, "Descripcion": "Balance Inicial",    "VActiva": false }
]

## ACCESO A LA APLICACION: 
## https://ziurappblazor-b3hdhbejangrd9h6.canadacentral-01.azurewebsites.net/

## Arquitectura

Separación por capas y proyectos:

- Domain: Entidades y contratos del dominio (agnósticos de infraestructura).

- Application: Casos de uso (Queries/Commands) con MediatR; puertos (interfaces).

- Infrastructure: Adaptadores (repos externos), HttpClient tipado, Polly, mapeos y configuración.

- Web: Blazor Web App + Minimal API; DI de Application/Infrastructure; páginas Razor para la UI.

- Endpoints internos (Minimal API)


- BlazorAppRest/
  - src/
    - Domain/
      - Documentos/
        - DocumentType.cs
    - Application/
      - Abstractions/
        - Repositories/
          - IDocumentTypesReadOnlyRepository.cs
      - DocumentTypes/
        - Queries/
          - GetAllDocumentTypesQuery.cs
          - GetAllDocumentTypesQueryHandler.cs
    - Infrastructure/
      - Options/
        - ZiurApiOptions.cs
      - Http/
        - ZiurApiClient.cs
      - Models/
        - ZiurDocumentTypeDto.cs
      - Adapters/
        - DocumentTypesReadOnlyRepository.cs
      - DependencyInjection.cs
    - Web/
      - BlazorAppRest/
        - Program.cs
        - Components/
          - Pages/
            - Documents/
              - Documents.razor
              - Documents.razor.css
- BlazorAppRest.sln


## GET /api/document-types
Devuelve { Codigo, Descripcion, Activa }[] ordenados ascendente por Codigo (orden aplicado en el QueryHandler).

## Prerrequisitos

- .NET SDK 8

- Acceso a Internet para consumir el endpoint Ziur

- Token válido (configurado por Secret Manager o variables de entorno)

## Paquetes NuGet clave

- MediatR

- Microsoft.Extensions.Http.Polly, Polly, Polly.Extensions.Http

- Microsoft.Extensions.Options.DataAnnotations
