using WakeProdutos.Domain.Entities;

namespace WakeProdutos.Domain.Interfaces
{
    public interface IProdutoRepository
    {
        Task AdicionarAsync(Produto produto);
        Task AtualizarAsync(Produto produto);
        Task DeletarAsync(Produto produto);
        Task<Produto?> ObterPorIdAsync(long id);
        Task<IEnumerable<Produto>> ObterListaComFiltrosAsync(string? nome = null, string? ordenarPor = null);
    }
}
