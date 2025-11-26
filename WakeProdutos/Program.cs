using Scalar.AspNetCore;
using WakeProdutos.API.Extensions;
using WakeProdutos.API.Filters;
using WakeProdutos.API.Middleware;
using WakeProdutos.Application;
using WakeProdutos.Infrastructure;
using WakeProdutos.Infrastructure.Data.Context;
using WakeProdutos.Infrastructure.Data.Seed;
using WakeProdutos.Shared.Constants;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers(options =>
{
    options.Filters.Add<ValidationFilterAttribute>();
});

builder.AddApplicationServices();
builder.AddInfrastructureServices();
builder.AddWebServices();

var app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>();

using (var scope = app.Services.CreateScope())
{
    if (!app.Environment.IsEnvironment("IntegrationTests") && app.Environment.IsDevelopment())
    {
        var context = scope.ServiceProvider.GetRequiredService<WakeDbContext>();
        await context.Database.EnsureCreatedAsync();
        await WakeDbContextSeed.SeedAsync(context);
    }
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseSwagger();

app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", Constantes.ApiTitle);
    c.RoutePrefix = "swagger";
});

app.MapOpenApi();
app.MapScalarApiReference("/scalar", options =>
{
    options.Title = Constantes.ApiTitle;
    options.Theme = ScalarTheme.DeepSpace;
});

app.MapControllers();

await app.RunAsync();
