using WakeProdutos.Shared.Results;
using MediatR;
using WakeProdutos.Application.Dtos;

namespace WakeProdutos.Application.UseCases.Produtos.Queries.ProdutoPorId
{
    public record ObterProdutoPorIdQuery(long Id) : IRequest<Result<ListaProdutoDto>>;
}
