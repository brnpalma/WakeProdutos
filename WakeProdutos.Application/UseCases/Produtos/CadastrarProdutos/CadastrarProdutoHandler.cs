using MediatR;
using WakeProdutos.Application.Dtos;
using WakeProdutos.Domain.Entities;
using WakeProdutos.Domain.Interfaces;
using WakeProdutos.Shared.Results;

namespace WakeProdutos.Application.UseCases.Produtos.CadastrarProdutos
{
    public class CadastrarProdutoHandler(IProdutoRepository produtoRepository) : IRequestHandler<CadastrarProdutoCommand, Result<ProdutoDto>>
    {
        private readonly IProdutoRepository _produtoRepository = produtoRepository;

        public async Task<Result<ProdutoDto>> Handle(CadastrarProdutoCommand request, CancellationToken cancellationToken)
        {
            if (!Produto.CheckData(request.Nome, request.Estoque, request.Valor, out var produto, out var error))
            {
                return Result<ProdutoDto>.Fail(400, error ?? "Produto inválido.", null);
            }

            await _produtoRepository.AdicionarAsync(produto);

            var produtoDto = new ProdutoDto
            {
                Id = produto.Id,
                Nome = produto.Nome,
                Estoque = produto.Estoque,
                Valor = produto.Valor,
                Mensagem = "Produto cadastrado com sucesso",
                Sucesso = true
            };

            return Result<ProdutoDto>.Ok(produtoDto, 201);
        }
    }
}
