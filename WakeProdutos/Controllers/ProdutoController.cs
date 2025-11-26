using WakeProdutos.Shared.Results;
using Microsoft.AspNetCore.Mvc;
using WakeProdutos.Application.UseCases.Produtos.ListarProdutos;
using WakeProdutos.Shared.Constants;
using MediatR;
using WakeProdutos.Application.Dtos;
using WakeProdutos.Application.UseCases.Produtos.CadastrarProdutos;
using WakeProdutos.Application.UseCases.Produtos.AtualizarProduto;

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
        [ProducesResponseType(typeof(ProdutoDto), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(Result<ProdutoDto>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Result<ProdutoDto>), StatusCodes.Status500InternalServerError)]
        [EndpointDescription("Alterar dados de um produto já existente.")]
        public async Task<IActionResult> AtualizarProduto([FromRoute] long id, [FromBody] AtualizarProdutoDto request)
        {
            var command = new AtualizarProdutoCommand(id, request.Nome, request.Estoque, request.Valor);

            var result = await _sender.Send(command);
            return StatusCode(result.Status, result.Data is null ? result : result.Data);
        }

        [HttpGet("produtos")]
        [ProducesResponseType(typeof(ProdutoDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Result<ProdutoDto>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Result<ProdutoDto>), StatusCodes.Status500InternalServerError)]
        [EndpointDescription("Autentica um usuário e gera um token JWT. Retorna o token e informações básicas da sessão.")]
        public async Task<IActionResult> ListarProdutos()
        {
            var result = await _sender.Send(new ListarProdutosRequest());

            if (result.Data is null)
                return StatusCode(result.Status, result);

            return result.Data.Any()
                ? StatusCode(result.Status, result.Data)
                : Ok(new { Mensagem = "Nenhum produto cadastrado." });
        }
    }
}
