using WakeProdutos.Shared.Results;
using MediatR;
using WakeProdutos.Application.Dtos;

namespace WakeProdutos.Application.UseCases.Produtos.AtualizarProduto
{
    public record AtualizarProdutoCommand(long Id, string Nome, int Estoque, decimal Valor) : IRequest<Result<ProdutoDto>>;
}
