using MediatR;
using WakeProdutos.Application.Dtos;
using WakeProdutos.Domain.Interfaces;
using WakeProdutos.Shared.Results;

namespace WakeProdutos.Application.UseCases.Produtos.Queries.ProdutoPorId
{
    public class ObterProdutoPorIdQueryHandler(IProdutoRepository produtoRepository) : IRequestHandler<ObterProdutoPorIdQuery, Result<ListaProdutoDto>>
    {
        private readonly IProdutoRepository _produtoRepository = produtoRepository;

        public async Task<Result<ListaProdutoDto>> Handle(ObterProdutoPorIdQuery request, CancellationToken cancellationToken)
        {
            var produtoBanco = await _produtoRepository.ObterPorIdAsync(request.Id);

            if (produtoBanco is null)
            {
                return Result<ListaProdutoDto>.Fail(404, "Nenhum produto encontrado com este Id.", null);
            }

            var produtoDto = new ListaProdutoDto
            {
                Id = produtoBanco.Id,
                Nome = produtoBanco.Nome,
                Estoque = produtoBanco.Estoque,
                Valor = produtoBanco.Valor,
            };

            return Result<ListaProdutoDto>.Ok(produtoDto, 200);
        }
    }
}
