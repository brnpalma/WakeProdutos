using WakeProdutos.Shared.Results;
using MediatR;
using WakeProdutos.Application.Interfaces;
using WakeProdutos.Application.Dtos.Produtos;

namespace WakeProdutos.Application.UseCases.Produtos.ListarProdutos
{
    public class ListarProdutosHandler(IProdutoService produtoService) 
        : IRequestHandler<ListarProdutosRequest, Result<IEnumerable<ListaProdutoDto>>>
    {
        private readonly IProdutoService _produtoService = produtoService;

        public async Task<Result<IEnumerable<ListaProdutoDto>>> Handle(ListarProdutosRequest request, CancellationToken cancellationToken)
        {
            return await _produtoService.ListarAsync();
        }
    }
}
