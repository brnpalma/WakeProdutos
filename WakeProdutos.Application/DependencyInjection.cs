using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using FluentValidation;

namespace WakeProdutos.Application;

public static class DependencyInjection
{
    public static void AddApplicationServices(this IHostApplicationBuilder builder)
    {
        builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());
        builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        builder.Services.AddMediatR(cfg => {
            cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
        });
    }
}
