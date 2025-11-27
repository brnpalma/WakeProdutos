using MediatR;
using WakeProdutos.Application.Dtos;
using WakeProdutos.Domain.Interfaces;
using WakeProdutos.Shared.Results;

namespace WakeProdutos.Application.UseCases.Produtos.Queries.ListarProdutos
{
    public class ListarProdutosHandler(IProdutoRepository produtoRepository) 
        : IRequestHandler<ListarProdutosQuery, Result<IEnumerable<ListaProdutoDto>>>
    {
        private readonly IProdutoRepository _produtoRepository = produtoRepository;

        public async Task<Result<IEnumerable<ListaProdutoDto>>> Handle(ListarProdutosQuery request, CancellationToken cancellationToken)
        {
            var propsValidas = typeof(ListaProdutoDto)
                .GetProperties()
                .Select(p => p.Name.ToLower()).ToList();

            if (!string.IsNullOrWhiteSpace(request.OrdenarPor) 
                && !propsValidas.Contains(request.OrdenarPor.ToLower()))
            {
                var mensagem = $"Parâmetro 'ordenarPor' inválido. Valores possíveis: {string.Join(", ", propsValidas)}";
                return Result<IEnumerable<ListaProdutoDto>>.Fail(400, mensagem, null);
            }

            var produtos = await _produtoRepository.ObterListaComFiltrosAsync(request.Nome, request.OrdenarPor);

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
