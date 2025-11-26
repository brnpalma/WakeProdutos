using Microsoft.EntityFrameworkCore;
using WakeProdutos.Domain.Entities;
using WakeProdutos.Domain.Interfaces;
using WakeProdutos.Infrastructure.Data.Context;

namespace WakeProdutos.Infrastructure.Repositories
{
    public class ProdutoRepository(WakeDbContext context) : IProdutoRepository
    {
        private readonly WakeDbContext _context = context;

        public async Task AdicionarAsync(Produto produto)
        {
            _context.Produtos.Add(produto);
            await _context.SaveChangesAsync();
        }

        public async Task AtualizarAsync(Produto produto)
        {
            _context.Produtos.Update(produto);
            await _context.SaveChangesAsync();
        }

        public async Task<Produto?> ObterPorIdAsync(long id)
        {
            return await _context.Produtos.FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<IEnumerable<Produto>> ObterTodosAsync()
        {
            return await _context.Produtos.AsNoTracking().ToListAsync();
        }
    }
}
