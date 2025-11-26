using WakeProdutos.Shared.Results;
using MediatR;
using WakeProdutos.Application.Dtos;

namespace WakeProdutos.Application.UseCases.Produtos.ListarProdutos
{
    public record ListarProdutosRequest() : IRequest<Result<IEnumerable<ListaProdutoDto>>>;
}
