using WakeProdutos.Shared.Results;
using Microsoft.AspNetCore.Mvc;
using WakeProdutos.Shared.Constants;
using MediatR;
using WakeProdutos.Application.Dtos;
using WakeProdutos.Application.UseCases.Produtos.Commands.CadastrarProdutos;
using WakeProdutos.Application.UseCases.Produtos.Commands.AtualizarProduto;
using WakeProdutos.Application.UseCases.Produtos.Commands.DeletarProduto;
using WakeProdutos.Application.UseCases.Produtos.Queries.ProdutoPorId;
using WakeProdutos.Application.UseCases.Produtos.Queries.ListarProdutos;

namespace WakeProdutos.API.Controllers 
{
    [ApiController]
    [Route($"api/{Constantes.ApiVersion}")]
    public class ProdutoController(ISender sender) : ControllerBase
    {
        private readonly ISender _sender = sender;

        [HttpPost("produtos")]
        [ProducesResponseType(typeof(ProdutoDto), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(Result<ProdutoDto>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Result<ProdutoDto>), StatusCodes.Status500InternalServerError)]
        [EndpointDescription("Cria um novo produto. Recebe dados de cadastro e retorna o produto criado.")]
        public async Task<IActionResult> CadastrarProduto([FromBody] CadastrarProdutoCommand request)
        {
            var result = await _sender.Send(request);
            return StatusCode(result.Status, result.Data is null ? result : result.Data);
        }

        [HttpPut("produtos/{id}")]
        [ProducesResponseType(typeof(ProdutoDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Result<ProdutoDto>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Result<ProdutoDto>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Result<ProdutoDto>), StatusCodes.Status500InternalServerError)]
        [EndpointDescription("Alterar dados de um produto através de seu ID já existente.")]
        public async Task<IActionResult> AtualizarProduto([FromRoute] long id, [FromBody] AtualizarProdutoDto request)
        {
            var command = new AtualizarProdutoCommand(id, request.Nome, request.Estoque, request.Valor);

            var result = await _sender.Send(command);
            return StatusCode(result.Status, result.Data is null ? result : result.Data);
        }

        [HttpDelete("produtos/{id}")]
        [ProducesResponseType(typeof(ProdutoDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Result<ProdutoDto>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Result<ProdutoDto>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Result<ProdutoDto>), StatusCodes.Status500InternalServerError)]
        [EndpointDescription("Deletar um produto por Id.")]
        public async Task<IActionResult> DeletarProduto([FromRoute] long id)
        {
            var command = new DeletarProdutoCommand(id);

            var result = await _sender.Send(command);
            return StatusCode(result.Status, result.Data is null ? result : result.Data);
        }

        [HttpGet("produtos/{id}")]
        [ProducesResponseType(typeof(ProdutoDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Result<ProdutoDto>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Result<ProdutoDto>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Result<ProdutoDto>), StatusCodes.Status500InternalServerError)]
        [EndpointDescription("Consultar o cadastro de um produto por Id.")]
        public async Task<IActionResult> ObterProdutoPorId([FromRoute] long id)
        {
            var command = new ObterProdutoPorIdQuery(id);

            var result = await _sender.Send(command);
            return StatusCode(result.Status, result.Data is null ? result : result.Data);
        }

        [HttpGet("produtos")]
        [ProducesResponseType(typeof(ProdutoDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Result<ProdutoDto>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Result<ProdutoDto>), StatusCodes.Status500InternalServerError)]
        [EndpointDescription("Retorna uma lista de produtos com filtros opcionais de busca por nome e ordenação.")]
        public async Task<IActionResult> ListarProdutos([FromQuery] string? nome, [FromQuery] string? ordenarPor)
        {
            var result = await _sender.Send(new ListarProdutosCommand(nome, ordenarPor));

            if (result.Data is null)
                return StatusCode(result.Status, result);

            return result.Data.Any()
                ? StatusCode(result.Status, result.Data)
                : Ok(new { Mensagem = "Nenhum produto cadastrado." });
        }
    }
}
