using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using FluentAssertions;
using WakeProdutos.Application.Dtos;
using WakeProdutos.Application.UseCases.Produtos.Commands.CadastrarProdutos;
using Xunit;

namespace WakeProdutos.Tests.Integration;

public class ProdutosControllerTests(IntegrationTestsFactory factory) : IClassFixture<IntegrationTestsFactory>
{
    private readonly HttpClient _client = factory.CreateClient();

    [Fact]
    public async Task ListarProdutos_RetornaProdutosSeedados()
    {
        var response = await _client.GetAsync("/api/v1/produtos");
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var produtos = await response.Content.ReadFromJsonAsync<ProdutoDto[]>();

        produtos.Should().NotBeNull();
        produtos!.Length.Should().BeGreaterThan(0);
    }

    [Fact]
    public async Task ObterProdutoPorId_RetornaProduto()
    {
        var response = await _client.GetAsync("/api/v1/produtos/1");
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var produto = await response.Content.ReadFromJsonAsync<ProdutoDto>();

        produto.Should().NotBeNull();
        produto!.Id.Should().Be(1);
    }

    [Fact]
    public async Task CadastrarProduto_CriaProduto()
    {
        var command = new CadastrarProdutoCommand("Bola Futebol Americano", 10, 99.99m);
        var response = await _client.PostAsJsonAsync("/api/v1/produtos", command);
        response.StatusCode.Should().Be(HttpStatusCode.Created);

        var produto = await response.Content.ReadFromJsonAsync<ProdutoDto>();

        produto.Should().NotBeNull();
        produto!.Nome.Should().Be("Bola Futebol Americano");
    }

    [Fact]
    public async Task AtualizarProduto_AtualizaProduto()
    {
        // Primeiro criar
        var create = new CadastrarProdutoCommand("Miniatura F1 1:24", 5, 399.5m);
        var createResp = await _client.PostAsJsonAsync("/api/v1/produtos", create);
        createResp.StatusCode.Should().Be(HttpStatusCode.Created);

        var created = await createResp.Content.ReadFromJsonAsync<ProdutoDto>();

        created.Should().NotBeNull();
        var id = created!.Id;

        //Atualizar
        var updateDto = new { Nome = "Miniatura Prototipo 1:48", Estoque = 7, Valor = 159.0m };
        var putResp = await _client.PutAsJsonAsync($"/api/v1/produtos/{id}", updateDto);
        putResp.StatusCode.Should().Be(HttpStatusCode.Created);

        var updated = await putResp.Content.ReadFromJsonAsync<ProdutoDto>();

        updated.Should().NotBeNull();
        updated!.Nome.Should().Be("Miniatura Prototipo 1:48");
    }

    [Fact]
    public async Task DeletarProduto_ExcluiProdutoLogicamente()
    {
        // Primeiro criar
        var create = new CadastrarProdutoCommand("Lixeira de aluminio", 3, 20.0m);
        var createResp = await _client.PostAsJsonAsync("/api/v1/produtos", create);
        createResp.StatusCode.Should().Be(HttpStatusCode.Created);

        var created = await createResp.Content.ReadFromJsonAsync<ProdutoDto>();

        created.Should().NotBeNull();
        var id = created!.Id;

        // Deletar
        var delResp = await _client.DeleteAsync($"/api/v1/produtos/{id}");
        delResp.StatusCode.Should().Be(HttpStatusCode.OK);

        var deleted = await delResp.Content.ReadFromJsonAsync<ProdutoDto>();

        deleted.Should().NotBeNull();
        deleted!.Id.Should().Be(id);

        // Verificar exclusão lógica: consulta por id deve retornar NotFound
        var getResp = await _client.GetAsync($"/api/v1/produtos/{id}");
        getResp.StatusCode.Should().Be(HttpStatusCode.NotFound);

        // E o produto não deve aparecer na listagem
        var listResp = await _client.GetAsync("/api/v1/produtos");
        listResp.StatusCode.Should().Be(HttpStatusCode.OK);
        var produtos = await listResp.Content.ReadFromJsonAsync<ProdutoDto[]>();
        produtos.Should().NotBeNull();
        produtos!.Should().NotContain(p => p.Id == id);
    }
}
