using Tellus.Application.Core;
using Tellus.Application.Queries;
using Tellus.Application.Responses;
using Tellus.Core.Events;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Tellus.API.Controller.User
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class ClientController : ControllerBase
    {
        private readonly IMediator _mediator;
        public ClientController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpPost, AllowAnonymous]
        [Route("login")]
        [ProducesResponseType(typeof(ResultEvent), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Login([FromBody] AutenticacaoClienteQuery query)
        {
            try
            {
                var response = await _mediator.Send(query);
                return Ok(response);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
        [HttpPost, AllowAnonymous]
        [Route("refreshToken")]
        [ProducesResponseType(typeof(ResultEvent), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> RefreshToken([FromBody] ValidaClienteRefreshTokenQuery query)
        {
            try
            {
                var response = await _mediator.Send(query);
                return Ok(response);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
        [HttpPost]
        [ProducesResponseType(typeof(ResultEvent), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Adicionar([FromBody] AdicionarClienteQuery query)
        {
            try
            {
                var response = await _mediator.Send(query);
                return Ok(response);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
        [HttpPut]
        [ProducesResponseType(typeof(ResultEvent), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Atualizar([FromBody] AtualizarClienteQuery query)
        {
            try
            {
                var response = await _mediator.Send(query);
                return Ok(response);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
        [HttpPost]
        [Route("obterTodos")]
        [ProducesResponseType(typeof(List<ClienteResponse>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> ObterTodos([FromBody] ObterTodosClienteQuery query)
        {
            try
            {
                var response = await _mediator.Send(query);
                return Ok(response);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
        [HttpDelete]
        [Route("{id}")]
        [ProducesResponseType(typeof(ResultEvent), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> DeletarPorId(Guid id)
        {
            try
            {
                var response = await _mediator.Send(new DeletarClientePorIdQuery(id));
                return Ok(response);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType(typeof(ClienteResponse), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> ObterPorId(Guid id)
        {
            try
            {
                var response = await _mediator.Send(new ObterClientePorIdQuery(id));
                return Ok(response);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpPost]
        [Route("gerarNovaSenha")]
        [ProducesResponseType(typeof(ResultEvent), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GerarNovaSenha([FromBody] GerarNovaSenhaClienteQuery query)
        {
            try
            {
                var response = await _mediator.Send(query);
                return Ok(response);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
        [HttpGet]
        [Route("validarEsqueciSenhaViaLink/{p}")]
        [ProducesResponseType(typeof(ResultEvent), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> ValidarEsqueciSenhaViaLink(string p)
        {
            try
            {
                var response = await _mediator.Send(new ValidarEsqueciSenhaClienteViaLinkQuery(p));
                return Ok(response);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
        [HttpPost]
        [Route("esqueciSenhaAtualizarSenhaViaLink")]
        [ProducesResponseType(typeof(ResultEvent), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> EsqueciSenhaAtualizarSenhaViaLink([FromBody] AtualizarSenhaEsqueciSenhaClienteQuery query)
        {
            try
            {
                var response = await _mediator.Send(query);
                return Ok(response);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
        [HttpGet]
        [Route("validarCadastroViaLink/{p}")]
        [ProducesResponseType(typeof(ResultEvent), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> ValidarCadastroViaLink(string p)
        {
            try
            {
                var response = await _mediator.Send(new ValidarCadastroClienteViaLinkQuery(p));
                return Ok(response);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
        [HttpPost, AllowAnonymous]
        [Route("cadastrarSenha")]
        [ProducesResponseType(typeof(ResultEvent), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> CadastrarSenha([FromBody] CadastrarSenhaClienteQuery query)
        {
            try
            {
                var response = await _mediator.Send(query);
                return Ok(response);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
        [HttpGet]
        [Route("obterLogado")]
        [ProducesResponseType(typeof(ResultEvent), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> ObterLogado()
        {
            try
            {
                var response = await _mediator.Send(new ObterClienteLogadoQuery());
                return Ok(response);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
        [HttpPut]
        [Route("atualizarSenha")]
        [ProducesResponseType(typeof(ResultEvent), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> AtualizarSenha([FromBody] AtualizarSenhaClienteLogadoQuery query)
        {
            try
            {
                var response = await _mediator.Send(query);
                return Ok(response);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
        [HttpPost]
        [Route("gerarLog")]
        [ProducesResponseType(typeof(IndiceResponse), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GerarLog([FromBody] GerarLogClienteQuery query)
        {
            try
            {
                var response = await _mediator.Send(query);
                return Ok(response);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
        [HttpPost]
        [Route("obterLogados")]
        [ProducesResponseType(typeof(ResultEvent), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> ObterLogados([FromBody] ObterTodosUsuarioLogadoPorClienteIdQuery query)
        {
            try
            {
                var response = await _mediator.Send(query);
                return Ok(response);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
        [HttpPost]
        [Route("deslogarUsuario")]
        [ProducesResponseType(typeof(ResultEvent), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> DeslogarUsuario([FromBody] DeletarUsuarioLogadoPorIdQuery query)
        {
            try
            {
                var response = await _mediator.Send(query);
                return Ok(response);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
        [HttpPost]
        [Route("associarProduto")]
        [ProducesResponseType(typeof(ResultEvent), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> AssociarProduto([FromBody] AssociarClienteProdutoQuery query)
        {
            try
            {
                var response = await _mediator.Send(query);
                return Ok(response);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
        [HttpPut]
        [Route("atualizarChave")]
        [ProducesResponseType(typeof(ResultEvent), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> AtualizarChave([FromBody] AtualizarChaveClienteLogadoQuery query)
        {
            try
            {
                var response = await _mediator.Send(query);
                return Ok(response);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
        [HttpPost]
        [Route("obterConsultas")]
        [ProducesResponseType(typeof(ResultEvent), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> ObterLogado([FromBody] ObterConsumoClienteQuery query)
        {
            try
            {
                var response = await _mediator.Send(query);
                return Ok(response);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
        [HttpDelete]
        [Route("consulta/{id}")]
        [ProducesResponseType(typeof(ResultEvent), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> DeletarConsultaPorId(Guid id)
        {
            try
            {
                var response = await _mediator.Send(new DeletarConsumoClientePorIdQuery(id));
                return Ok(response);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
    }
}
