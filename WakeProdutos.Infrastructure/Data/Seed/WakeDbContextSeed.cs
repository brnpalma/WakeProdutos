using WakeProdutos.Domain.Entities;
using WakeProdutos.Infrastructure.Data.Context;

namespace WakeProdutos.Infrastructure.Data.Seed
{
    public static class WakeDbContextSeed
    {
        public static async Task SeedAsync(WakeDbContext context)
        {
            if (!context.Produtos.Any())
            {
                context.Produtos.AddRange(
                    new Produto("Tênis", 150, 390.90m),
                    new Produto("Perfume", 20, 180.20m),
                    new Produto("Jaqueta Moletom", 10, 199.99m),
                    new Produto("Teclado Gamer", 50, 259.59m),
                    new Produto("Volante Logitech G29", 5, 1999.99m)
                );

                await context.SaveChangesAsync();
            }
        }
    }
}
