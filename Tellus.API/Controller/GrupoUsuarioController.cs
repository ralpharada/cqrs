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
    public class GrupoUsuarioController : ControllerBase
    {
        private readonly IMediator _mediator;
        public GrupoUsuarioController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpPost]
        [ProducesResponseType(typeof(ResultEvent), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Adicionar([FromBody] AdicionarGrupoUsuarioQuery query)
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
        public async Task<IActionResult> Atualizar([FromBody] AtualizarGrupoUsuarioQuery query)
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
        [ProducesResponseType(typeof(List<GrupoUsuarioResponse>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> ObterTodosUsuarioPorClienteId([FromBody] ObterTodosGrupoUsuarioQuery query)
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
        [ProducesResponseType(typeof(List<GrupoUsuarioResponse>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> ObterTodosGrupoUsuarioCompleto()
        {
            try
            {
                var response = await _mediator.Send(new ObterTodosGrupoUsuarioCompletoQuery());
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
                var response = await _mediator.Send(new DeletarGrupoUsuarioPorIdQuery(id));
                return Ok(response);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType(typeof(GrupoUsuarioResponse), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> ObterPorId(Guid id)
        {
            try
            {
                var response = await _mediator.Send(new ObterGrupoUsuarioPorIdQuery(id));
                return Ok(response);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
        [HttpPost]
        [Route("bloquear")]
        [ProducesResponseType(typeof(ResultEvent), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> BloquearPorId([FromBody] BloquearGrupoUsuarioPorIdQuery query)
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
        [Route("obterTodosPorUsuario")]
        [ProducesResponseType(typeof(GrupoUsuarioResponse), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> ObterPorId([FromBody] ObterGrupoUsuarioPorUsuarioIdQuery query)
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
        [Route("associarUsuario")]
        [ProducesResponseType(typeof(ResultEvent), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> AssociarUsuario([FromBody] AssociarGrupoUsuarioUsuarioQuery query)
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
        public async Task<IActionResult> AssociarTipoDocumento([FromBody] AssociarGrupoUsuarioTipoDocumentoQuery query)
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
        public async Task<IActionResult> AssociarPermissao([FromBody] AssociarGrupoUsuarioPermissaoQuery query)
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
        [Route("associarTipoDocumentoPermissao")]
        [ProducesResponseType(typeof(ResultEvent), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> AssociarTipoDocumentoPermissao([FromBody] AssociarGrupoUsuarioVinculoPermissaoQuery query)
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
        public async Task<IActionResult> AssociarProduto([FromBody] AssociarGrupoUsuarioProdutoQuery query)
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
