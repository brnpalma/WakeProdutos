using Scalar.AspNetCore;
using System.Reflection;
using WakeProdutos.API;
using WakeProdutos.API.Filters;
using WakeProdutos.API.Middleware;
using WakeProdutos.Application;
using WakeProdutos.Infrastructure;
using WakeProdutos.Infrastructure.Persistence;
using WakeProdutos.Shared.Constants;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers(options =>
{
    options.Filters.Add<ValidateModelAttribute>();
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
