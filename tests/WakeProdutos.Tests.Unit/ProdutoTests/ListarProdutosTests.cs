using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using WakeProdutos.Application.UseCases.Produtos.Queries.ListarProdutos;
using WakeProdutos.Domain.Entities;
using WakeProdutos.Domain.Interfaces;
using Xunit;

namespace WakeProdutos.Tests.Unit.ProdutoTests;

public class ListarProdutosTests
{
    [Fact(DisplayName = "Listar produtos sem filtros, deve retornar lista de todos.")]
    public async Task ListarProdutos_SemFiltros_DeveRetornarLista()
    {
        var produtos = new List<Produto>
        {
            new ("Notebook Dell Inspiron 15", 1, 3999.99m){ Id = 1 },
            new ("Cafeteira Nespresso Essenza Mini", 2, 750.50m){ Id = 2 },
            new ("Tênis Nike Air Max", 2, 599.99m){ Id = 3 },
            new ("Fone de Ouvido JBL Tune 510BT", 2, 204.70m){ Id = 4 },
            new ("Liquidificador Mondial", 2, 79.90m){ Id = 2 }
        };

        var repoMock = new Mock<IProdutoRepository>();
        repoMock.Setup(r => r.ObterListaComFiltrosAsync(null, null)).ReturnsAsync(produtos);

        var handler = new ListarProdutosHandler(repoMock.Object);
        var query = new ListarProdutosQuery();

        var result = await handler.Handle(query, default);

        result.Sucesso.Should().BeTrue();
        result.Status.Should().Be(200);
        result.Data.Should().NotBeNull();
        result.Data!.Count().Should().Be(5);
    }

    [Fact(DisplayName = "Listar produtos com parametro ordenarPor inválido, deve retornar HTTP 400.")]
    public async Task ListarProdutos_OrdenarPorInvalido_DeveRetornar400()
    {
        var repoMock = new Mock<IProdutoRepository>();
        var handler = new ListarProdutosHandler(repoMock.Object);
        var query = new ListarProdutosQuery(null, "invalido");

        var result = await handler.Handle(query, default);

        result.Sucesso.Should().BeFalse();
        result.Status.Should().Be(400);
    }
}
