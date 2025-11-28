using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using WakeProdutos.Application.UseCases.Produtos.Commands.DeletarProduto;
using WakeProdutos.Domain.Entities;
using WakeProdutos.Domain.Interfaces;
using Xunit;

namespace WakeProdutos.Tests.Unit.ProdutoTests;

public class DeletarProdutoTests
{
    [Fact(DisplayName = "Deletar produto existente, deve retornar sucesso.")]
    public async Task DeletarProduto_Existente_DeveRetornarSucesso()
    {
        var produto = new Produto("Bola Nike total 90", 2, 10m) { Id = 3 };
        var repoMock = new Mock<IProdutoRepository>();
        repoMock.Setup(r => r.ObterPorIdAsync(3)).ReturnsAsync(produto);

        Produto? captured = null;
        repoMock.Setup(r => r.DeletarAsync(It.IsAny<Produto>()))
            .Callback<Produto>(p => captured = p)
            .Returns(Task.CompletedTask);

        var handler = new DeletarProdutoHandler(repoMock.Object);
        var command = new DeletarProdutoCommand(3);

        var result = await handler.Handle(command, default);

        result.Sucesso.Should().BeTrue();
        result.Status.Should().Be(200);
        result.Data.Should().NotBeNull();
        result.Data!.Id.Should().Be(3);

        // Verifica que o repositório foi chamado com um produto marcado como excluído (exclusão lógica)
        captured.Should().NotBeNull();
        captured!.Excluido.Should().BeTrue();
    }

    [Fact(DisplayName = "Deletar produto inexistente, deve retornar HTTP 404.")]
    public async Task DeletarProduto_Inexistente_DeveRetornar404()
    {
        var repoMock = new Mock<IProdutoRepository>();
        repoMock.Setup(r => r.ObterPorIdAsync(999)).ReturnsAsync((Produto?)null);

        var handler = new DeletarProdutoHandler(repoMock.Object);
        var command = new DeletarProdutoCommand(999);

        var result = await handler.Handle(command, default);

        result.Sucesso.Should().BeFalse();
        result.Status.Should().Be(404);
    }
}
