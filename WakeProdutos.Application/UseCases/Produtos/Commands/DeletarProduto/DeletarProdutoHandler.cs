using MediatR;
using WakeProdutos.Application.Dtos;
using WakeProdutos.Domain.Interfaces;
using WakeProdutos.Shared.Results;

namespace WakeProdutos.Application.UseCases.Produtos.Commands.DeletarProduto
{
    public class DeletarProdutoHandler(IProdutoRepository produtoRepository) : IRequestHandler<DeletarProdutoCommand, Result<ProdutoDto>>
    {
        private readonly IProdutoRepository _produtoRepository = produtoRepository;

        public async Task<Result<ProdutoDto>> Handle(DeletarProdutoCommand request, CancellationToken cancellationToken)
        {
            var produtoBanco = await _produtoRepository.ObterPorIdAsync(request.Id);

            if (produtoBanco is null)
            {
                return Result<ProdutoDto>.Fail(404, "Nenhum produto encontrado com este Id.", null);
            }

            // Soft-delete: marcar o produto como excluído em vez de removê-lo fisicamente
            produtoBanco.Excluido = true;
            await _produtoRepository.DeletarAsync(produtoBanco);

            var produtoDto = new ProdutoDto
            {
                Id = produtoBanco.Id,
                Nome = produtoBanco.Nome,
                Estoque = produtoBanco.Estoque,
                Valor = produtoBanco.Valor,
                Mensagem = "Produto deletado.",
                Sucesso = true
            };

            return Result<ProdutoDto>.Ok(produtoDto, 200);
        }
    }
}
