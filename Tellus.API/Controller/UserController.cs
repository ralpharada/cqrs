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
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;
        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpPost, AllowAnonymous]
        [Route("login")]
        [ProducesResponseType(typeof(ResultEvent), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Login([FromBody] AutenticacaoUsuarioQuery query)
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
        public async Task<IActionResult> RefreshToken([FromBody] ValidaUsuarioRefreshTokenQuery query)
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
        public async Task<IActionResult> Adicionar([FromBody] AdicionarUsuarioQuery query)
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
        public async Task<IActionResult> Atualizar([FromBody] AtualizarUsuarioQuery query)
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
        [Route("atualizarSenha")]
        [ProducesResponseType(typeof(ResultEvent), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> AtualizarSenha([FromBody] AtualizarSenhaUsuarioQuery query)
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
        [ProducesResponseType(typeof(List<UsuarioResponse>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> ObterTodosUsuarioPorClienteId([FromBody] ObterTodosUsuarioPorClienteIdQuery query)
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
        [Route("obterTodosCompleto")]
        [ProducesResponseType(typeof(List<UsuarioResponse>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> ObterTodosCompleto()
        {
            try
            {
                var response = await _mediator.Send(new ObterTodosUsuarioCompletoQuery());
                return Ok(response);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
        [HttpPost]
        [Route("obterTodosPorGrupoUsuario")]
        [ProducesResponseType(typeof(List<UsuarioResponse>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> ObterTodosUsuarioPorGrupoUsuarioId([FromBody] ObterTodosUsuarioPorGrupoUsuarioIdQuery query)
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
                var response = await _mediator.Send(new DeletarUsuarioPorIdQuery(id));
                return Ok(response);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType(typeof(UsuarioResponse), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> ObterPorId(Guid id)
        {
            try
            {
                var response = await _mediator.Send(new ObterUsuarioPorIdQuery(id));
                return Ok(response);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpPost, AllowAnonymous]
        [Route("gerarNovaSenha")]
        [ProducesResponseType(typeof(ResultEvent), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GerarNovaSenha([FromBody] GerarNovaSenhaUsuarioQuery query)
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
        [HttpGet, AllowAnonymous]
        [Route("validarEsqueciSenhaViaLink/{p}")]
        [ProducesResponseType(typeof(ResultEvent), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> ValidarEsqueciSenhaViaLink(string p)
        {
            try
            {
                var response = await _mediator.Send(new ValidarEsqueciSenhaUsuarioViaLinkQuery(p));
                return Ok(response);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
        [HttpPost, AllowAnonymous]
        [Route("esqueciSenhaAtualizarSenhaViaLink")]
        [ProducesResponseType(typeof(ResultEvent), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> EsqueciSenhaAtualizarSenhaViaLink([FromBody] EsqueciSenhaAtualizarSenhaViaLinkUsuarioQuery query)
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
        [HttpGet, AllowAnonymous]
        [Route("validarCadastroViaLink/{p}")]
        [ProducesResponseType(typeof(ResultEvent), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> ValidarCadastroViaLink(string p)
        {
            try
            {
                var response = await _mediator.Send(new ValidarCadastroUsuarioViaLinkQuery(p));
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
        public async Task<IActionResult> CadastrarSenha([FromBody] CadastrarSenhaUsuarioQuery query)
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
        [Route("associarGrupoUsuario")]
        [ProducesResponseType(typeof(ResultEvent), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> AssociarGrupoUsuario([FromBody] AssociarUsuarioGrupoUsuarioQuery query)
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
        [Route("associarTipoDocumento")]
        [ProducesResponseType(typeof(ResultEvent), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> AssociarTipoDocumento([FromBody] AssociarUsuarioTipoDocumentoQuery query)
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
        [Route("associarPermissao")]
        [ProducesResponseType(typeof(ResultEvent), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> AssociarPermissao([FromBody] AssociarUsuarioPermissaoQuery query)
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
                var response = await _mediator.Send(new ObterUsuarioLogadoQuery());
                return Ok(response);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
        [HttpPost]
        [Route("associarTipoDocumentoPermissao")]
        [ProducesResponseType(typeof(ResultEvent), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> AssociarTipoDocumentoPermissao([FromBody] AssociarUsuarioVinculoPermissaoQuery query)
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
        public async Task<IActionResult> AssociarProduto([FromBody] AssociarUsuarioProdutoQuery query)
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
    }
}
