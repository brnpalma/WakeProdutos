using WakeProdutos.Shared.Results;
using MediatR;
using WakeProdutos.Application.Dtos;

namespace WakeProdutos.Application.UseCases.Produtos.Commands.CadastrarProdutos
{
    public record CadastrarProdutoCommand(string Nome, int Estoque, decimal Valor) : IRequest<Result<ProdutoDto>>;
}
