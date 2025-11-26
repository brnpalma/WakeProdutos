using Microsoft.EntityFrameworkCore;
using WakeProdutos.Domain.Entities;

namespace WakeProdutos.Infrastructure.Data.Context
{
    public class WakeDbContext(DbContextOptions<WakeDbContext> options) : DbContext(options)
    {
        public DbSet<Produto> Produtos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Produto>(builder =>
            {
                builder.Property(p => p.Nome).IsRequired();
                builder.Property(p => p.Valor).HasPrecision(18, 2);
            });
        }
    }
}
