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

        public async Task DeletarAsync(Produto produto)
        {
            _context.Produtos.Update(produto);
            await _context.SaveChangesAsync();
        }

        public async Task<Produto?> ObterPorIdAsync(long id)
        {
            // Não retornar produtos marcados como excluídos
            return await _context.Produtos.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id && !p.Excluido);
        }

        public async Task<IEnumerable<Produto>> ObterListaComFiltrosAsync(string? nome = null, string? ordenarPor = null)
        {
            // Filtrar produtos excluídos
            IQueryable<Produto> query = _context.Produtos.AsNoTracking().Where(p => !p.Excluido);

            if (!string.IsNullOrWhiteSpace(nome))
            {
                query = query.Where(p => p.Nome.Contains(nome));
            }

            query = ordenarPor?.ToLower() switch
            {
                "nome" => query.OrderBy(p => p.Nome),
                "estoque" => query.OrderBy(p => p.Estoque),
                "valor" => query.OrderBy(p => p.Valor),
                _ or "id" => query.OrderBy(p => p.Id)
            };

            return await query.ToListAsync();
        }
    }
}
