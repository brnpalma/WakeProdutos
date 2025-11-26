using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using WakeProdutos.Shared.Constants;

namespace WakeProdutos.API.Extensions;

public static class DependencyInjection
{
    public static void AddWebServices(this IHostApplicationBuilder builder)
    {
        builder.Services.AddHttpContextAccessor();

        builder.Services.Configure<ApiBehaviorOptions>(options =>
            options.SuppressModelStateInvalidFilter = true);

        builder.Services.AddEndpointsApiExplorer();

        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = Constantes.ApiTitle,
                Version = "v1",
                Description = Constantes.ApiDescription
            });
        });

        builder.Services.AddOpenApi(options =>
        {
            options.AddDocumentTransformer((document, context, cancellationToken) =>
            {
                document.Info = new()
                {
                    Title = Constantes.ApiTitle,
                    Version = Constantes.ApiVersion,
                    Description = Constantes.ApiDescription
                };
                return Task.CompletedTask;
            });
        });
    }
}
