using WakeProdutos.Domain.Entities;

namespace WakeProdutos.Domain.Interfaces
{
    public interface IProdutoRepository
    {
        Task AdicionarAsync(Produto produto);
        Task AtualizarAsync(Produto produto);
        Task<Produto?> ObterPorIdAsync(long id);
        Task<IEnumerable<Produto>> ObterTodosAsync();
    }
}
