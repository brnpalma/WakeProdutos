using WakeProdutos.Shared.Results;
using MediatR;
using WakeProdutos.Application.Dtos;

namespace WakeProdutos.Application.UseCases.Produtos.Queries.ListarProdutos
{
    public record ListarProdutosQuery(string? Nome = null, string? OrdenarPor = null) : IRequest<Result<IEnumerable<ListaProdutoDto>>>;
}
