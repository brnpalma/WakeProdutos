using WakeProdutos.Domain.Entities;

namespace WakeProdutos.Application.Interfaces
{
    public interface IProdutoRepository
    {
        Task<bool> ExistePorNomeAsync(string nome);
        Task AdicionarAsync(Produto produto);
        Task<Produto?> ObterPorIdAsync(long id);
        Task<IEnumerable<Produto>> ObterTodosAsync();
    }
}
