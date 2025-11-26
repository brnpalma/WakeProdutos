using WakeProdutos.Shared.Results;
using MediatR;
using WakeProdutos.Application.Dtos.Produtos;

namespace WakeProdutos.Application.UseCases.Produtos.CadastrarProdutos
{
    public record CadastrarProdutosRequest(string Nome, int Estoque, decimal Valor) : IRequest<Result<ProdutoDto>>;
}
