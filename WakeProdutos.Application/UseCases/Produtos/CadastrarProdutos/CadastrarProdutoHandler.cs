using WakeProdutos.Shared.Results;
using MediatR;
using WakeProdutos.Application.Interfaces;
using WakeProdutos.Application.Dtos.Produtos;

namespace WakeProdutos.Application.UseCases.Produtos.CadastrarProdutos
{
    public class CadastrarProdutoHandler(IProdutoService authService) : IRequestHandler<CadastrarProdutosRequest, Result<ProdutoDto>>
    {
        private readonly IProdutoService _produtoService = authService;

        public async Task<Result<ProdutoDto>> Handle(CadastrarProdutosRequest request, CancellationToken cancellationToken)
        {
            return await _produtoService.CadastrarAsync(request.Nome, request.Estoque, request.Valor);
        }
    }
}
