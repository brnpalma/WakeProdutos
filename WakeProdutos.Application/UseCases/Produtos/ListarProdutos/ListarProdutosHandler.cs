using MediatR;
using WakeProdutos.Application.Dtos;
using WakeProdutos.Domain.Interfaces;
using WakeProdutos.Shared.Results;

namespace WakeProdutos.Application.UseCases.Produtos.ListarProdutos
{
    public class ListarProdutosHandler(IProdutoRepository produtoRepository) 
        : IRequestHandler<ListarProdutosRequest, Result<IEnumerable<ListaProdutoDto>>>
    {
        private readonly IProdutoRepository _produtoRepository = produtoRepository;

        public async Task<Result<IEnumerable<ListaProdutoDto>>> Handle(ListarProdutosRequest request, CancellationToken cancellationToken)
        {
            var produtos = await _produtoRepository.ObterTodosAsync();

            var produtosDto = produtos
                .Select(p => new ListaProdutoDto
                {
                    Id = p.Id,
                    Nome = p.Nome,
                    Estoque = p.Estoque,
                    Valor = p.Valor,
                })
                .ToList();

            return Result<IEnumerable<ListaProdutoDto>>.Ok(produtosDto, 200);
        }
    }
}
