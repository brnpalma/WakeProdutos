using WakeProdutos.Shared.Results;
using System.Data;
using WakeProdutos.Application.Interfaces;
using WakeProdutos.Application.Dtos.Produtos;
using WakeProdutos.Domain.Entities;

namespace WakeProdutos.Application.UseCases.Produtos.Services
{
    public class ProdutoService(IProdutoRepository produtoRepository) : IProdutoService
    {
        private readonly IProdutoRepository _produtoRepository = produtoRepository;

        public async Task<Result<ProdutoDto>> CadastrarAsync(string nome, int estoque, decimal valor)
        {
            if (await _produtoRepository.ExistePorNomeAsync(nome))
                return Result<ProdutoDto>.Fail(400, "Já existe um produto com este nome.", null);

            if (!Produto.TryCreate(nome, estoque, valor, out var produto, out var error))
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

        public async Task<Result<IEnumerable<ListaProdutoDto>>> ListarAsync()
        {
            var produtos = await _produtoRepository.ObterTodosAsync();

            var produtosDto = produtos
                .Select(p => new ListaProdutoDto
                {
                    Id = p.Id,
                    Nome = p.Nome,
                    Peso = p.Estoque,
                })
                .ToList();

            return Result<IEnumerable<ListaProdutoDto>>.Ok(produtosDto, 200);
        }
    }
}
