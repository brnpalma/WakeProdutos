using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using WakeProdutos.Application.UseCases.Produtos.Commands.AtualizarProduto;
using WakeProdutos.Domain.Interfaces;
using WakeProdutos.Domain.Entities;
using Xunit;

namespace WakeProdutos.Tests.Unit.ProdutoTests;

public class AtualizarProdutoTests
{
    [Fact(DisplayName = "Atualizar produto existente com dados válidos, deve retornar sucesso.")]
    public async Task AtualizarProduto_Existente_DadosValidos_DeveRetornarSucesso()
    {
        var produto = new Produto("Livro Gelo e Fogo", 5, 20m) { Id = 1 };
        var repoMock = new Mock<IProdutoRepository>();
        repoMock.Setup(r => r.ObterPorIdAsync(1)).ReturnsAsync(produto);
        repoMock.Setup(r => r.AtualizarAsync(It.IsAny<Produto>())).Returns(Task.CompletedTask);

        var handler = new AtualizarProdutoHandler(repoMock.Object);
        var command = new AtualizarProdutoCommand(1, "Livro Atualizado", 10, 25m);

        var result = await handler.Handle(command, default);

        result.Sucesso.Should().BeTrue();
        result.Status.Should().Be(200);
        result.Data.Should().NotBeNull();
        result.Data!.Nome.Should().Be("Livro Atualizado");
    }

    [Fact(DisplayName = "Atualizar produto inexistente, deve retornar HTTP 404.")]
    public async Task AtualizarProduto_Inexistente_DeveRetornar404()
    {
        var repoMock = new Mock<IProdutoRepository>();
        repoMock.Setup(r => r.ObterPorIdAsync(99)).ReturnsAsync((Produto?)null);

        var handler = new AtualizarProdutoHandler(repoMock.Object);
        var command = new AtualizarProdutoCommand(99, "Nome Qualquer", 1, 1m);

        var result = await handler.Handle(command, default);

        result.Sucesso.Should().BeFalse();
        result.Status.Should().Be(404);
    }

    [Fact(DisplayName = "Atualizar produto com dados inválidos, deve retornar HTTP 400.")]
    public async Task AtualizarProduto_DadosInvalidos_DeveRetornar400()
    {
        var produto = new Produto("Produto Invalido", 1, 1m) { Id = 2 };
        var repoMock = new Mock<IProdutoRepository>();
        repoMock.Setup(r => r.ObterPorIdAsync(2)).ReturnsAsync(produto);

        var handler = new AtualizarProdutoHandler(repoMock.Object);
        var command = new AtualizarProdutoCommand(2, "", 1, -5m);

        var result = await handler.Handle(command, default);

        result.Sucesso.Should().BeFalse();
        result.Status.Should().Be(400);
    }
}
