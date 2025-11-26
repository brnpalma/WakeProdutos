using WakeProdutos.Shared.Results;
using WakeProdutos.Application.Dtos.Produtos;

namespace WakeProdutos.Application.Interfaces
{
    public interface IProdutoService
    {
        Task<Result<ProdutoDto>> CadastrarAsync(string nome, int estoque, decimal valor);
        Task<Result<IEnumerable<ListaProdutoDto>>> ListarAsync();
    }
}
