using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using WakeProdutos.Application.UseCases.Produtos.Queries.ProdutoPorId;
using WakeProdutos.Domain.Entities;
using WakeProdutos.Domain.Interfaces;
using Xunit;

namespace WakeProdutos.Tests.Unit.ProdutoTests;

public class ObterProdutoPorIdTests
{
    [Fact(DisplayName = "Obter produto com ID existente, deve retornar o produto corretamente.")]
    public async Task ObterProdutoPorId_Existente_DeveRetornarProduto()
    {
        var produto = new Produto("Processador Intel I5", 1, 5m) { Id = 10 };
        var repoMock = new Mock<IProdutoRepository>();
        repoMock.Setup(r => r.ObterPorIdAsync(10)).ReturnsAsync(produto);

        var handler = new ObterProdutoPorIdQueryHandler(repoMock.Object);
        var query = new ObterProdutoPorIdQuery(10);

        var result = await handler.Handle(query, default);

        result.Sucesso.Should().BeTrue();
        result.Status.Should().Be(200);
        result.Data.Should().NotBeNull();
        result.Data!.Id.Should().Be(10);
    }

    [Fact(DisplayName = "Obter produto com ID inexistente, deve retornar HTTP 404.")]
    public async Task ObterProdutoPorId_Inexistente_DeveRetornar404()
    {
        var repoMock = new Mock<IProdutoRepository>();
        repoMock.Setup(r => r.ObterPorIdAsync(404)).ReturnsAsync((Produto?)null);

        var handler = new ObterProdutoPorIdQueryHandler(repoMock.Object);
        var query = new ObterProdutoPorIdQuery(404);

        var result = await handler.Handle(query, default);

        result.Sucesso.Should().BeFalse();
        result.Status.Should().Be(404);
    }
}
