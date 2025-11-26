using WakeProdutos.Shared.Results;
using MediatR;
using WakeProdutos.Application.Dtos;

namespace WakeProdutos.Application.UseCases.Produtos.DeletarProduto
{
    public record DeletarProdutoCommand(long Id) : IRequest<Result<ProdutoDto>>;
}
