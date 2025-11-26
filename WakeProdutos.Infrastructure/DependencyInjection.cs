using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WakeProdutos.Infrastructure.Persistence;
using WakeProdutos.Infrastructure.Repositories;
using WakeProdutos.Application.Interfaces;
using Ardalis.GuardClauses;

namespace WakeProdutos.Infrastructure;

public static class DependencyInjection
{
    public static void AddInfrastructureServices(this IHostApplicationBuilder builder)
    {
        var connectionString = builder.Configuration.GetConnectionString("WakeDb");
        Guard.Against.Null(connectionString, message: "Connection string 'WakeDb' não encontrada.");

        builder.Services.AddDbContext<WakeDbContext>(options =>
            options.UseSqlServer(connectionString));

        // Repositórios
        builder.Services.AddScoped<IProdutoRepository, ProdutoRepository>();

        builder.Services.AddSingleton(TimeProvider.System);

        builder.Services.AddDatabaseDeveloperPageExceptionFilter();
    }
}
