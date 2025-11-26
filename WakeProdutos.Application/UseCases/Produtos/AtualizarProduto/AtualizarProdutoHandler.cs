using MediatR;
using WakeProdutos.Application.Dtos;
using WakeProdutos.Domain.Entities;
using WakeProdutos.Domain.Interfaces;
using WakeProdutos.Shared.Results;

namespace WakeProdutos.Application.UseCases.Produtos.AtualizarProduto
{
    public class AtualizarProdutoHandler(IProdutoRepository produtoRepository) : IRequestHandler<AtualizarProdutoCommand, Result<ProdutoDto>>
    {
        private readonly IProdutoRepository _produtoRepository = produtoRepository;

        public async Task<Result<ProdutoDto>> Handle(AtualizarProdutoCommand request, CancellationToken cancellationToken)
        {
            var produtoBanco = await _produtoRepository.ObterPorIdAsync(request.Id);

            if (produtoBanco is null)
            {
                return Result<ProdutoDto>.Fail(400, "Nenhum produto encontrado com este Id.", null);
            }

            if (!Produto.CheckData(request.Nome, request.Estoque, request.Valor, out var produtoAtualizado, out var error))
            {
                return Result<ProdutoDto>.Fail(400, error ?? "Produto inválido.", null);
            }

            produtoBanco.Nome = produtoAtualizado.Nome;
            produtoBanco.Estoque = produtoAtualizado.Estoque;
            produtoBanco.Valor = produtoAtualizado.Valor;

            await _produtoRepository.AtualizarAsync(produtoBanco);

            var produtoDto = new ProdutoDto
            {
                Id = produtoBanco.Id,
                Nome = produtoBanco.Nome,
                Estoque = produtoBanco.Estoque,
                Valor = produtoBanco.Valor,
                Mensagem = "Produto atualizado com sucesso",
                Sucesso = true
            };

            return Result<ProdutoDto>.Ok(produtoDto, 201);
        }
    }
}
