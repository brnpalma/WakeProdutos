using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Linq;
using WakeProdutos.Infrastructure.Data.Context;
using WakeProdutos.Infrastructure.Data.Seed;

namespace WakeProdutos.Tests.Integration;

public class IntegrationTestsFactory : WebApplicationFactory<Program>
{
    protected override IHost CreateHost(IHostBuilder builder)
    {
        builder.UseEnvironment("IntegrationTests");

        builder.ConfigureServices(services =>
        {
            var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<WakeDbContext>));
            if (descriptor != null)
            {
                services.Remove(descriptor);
            }

            services.AddDbContext<WakeDbContext>(options =>
            {
                options.UseInMemoryDatabase("WakeProdutos_TestDb");
            });

            var sp = services.BuildServiceProvider();

            using (var scope = sp.CreateScope())
            {
                var scopedServices = scope.ServiceProvider;
                var db = scopedServices.GetRequiredService<WakeDbContext>();

                db.Database.EnsureDeleted();
                db.Database.EnsureCreated();
                WakeDbContextSeed.SeedAsync(db).GetAwaiter().GetResult();
            }
        });

        return base.CreateHost(builder);
    }
}
