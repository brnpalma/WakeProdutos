using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using WakeProdutos.Application.UseCases.Produtos.Commands.CadastrarProdutos;
using WakeProdutos.Domain.Interfaces;
using WakeProdutos.Domain.Entities;
using Xunit;

namespace WakeProdutos.Tests.Unit.ProdutoTests;

public class CadastrarProdutoTests
{
    [Fact(DisplayName = "Cadastrar produto com dados válidos, deve retornar sucesso.")]
    public async Task CadastrarProduto_ComDadosValidos_DeveRetornarSucesso()
    {
        var repoMock = new Mock<IProdutoRepository>();
        repoMock.Setup(r => r.AdicionarAsync(It.IsAny<Produto>())).Returns(Task.CompletedTask);

        var handler = new CadastrarProdutoHandler(repoMock.Object);

        var command = new CadastrarProdutoCommand("Caneta BIC ponta fina", 10, 2.50m);

        var result = await handler.Handle(command, default);

        result.Sucesso.Should().BeTrue();
        result.Status.Should().Be(201);
        result.Data.Should().NotBeNull();
        result.Data!.Nome.Should().Be("Caneta BIC ponta fina");
    }

    [Theory(DisplayName = "Cadastrar produto com nome inválido, deve retornar erro.")]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public async Task CadastrarProduto_NomeInvalido_DeveRetornarErro(string nome)
    {
        var repoMock = new Mock<IProdutoRepository>();
        var handler = new CadastrarProdutoHandler(repoMock.Object);

        var command = new CadastrarProdutoCommand(nome!, 10, 2.50m);

        var result = await handler.Handle(command, default);

        result.Sucesso.Should().BeFalse();
        result.Status.Should().Be(400);
    }

    [Fact(DisplayName = "Cadastrar produto com valor negativo, deve retornar erro.")]
    public async Task CadastrarProduto_ValorNegativo_DeveRetornarErro()
    {
        var repoMock = new Mock<IProdutoRepository>();
        var handler = new CadastrarProdutoHandler(repoMock.Object);

        var command = new CadastrarProdutoCommand("Caderno 100 folhas", 10, -1m);

        var result = await handler.Handle(command, default);

        result.Sucesso.Should().BeFalse();
        result.Status.Should().Be(400);
    }
}
