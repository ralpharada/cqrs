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
    public class TipoDocumentoGrupoController : ControllerBase
    {
        private readonly IMediator _mediator;
        public TipoDocumentoGrupoController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpPost]
        [ProducesResponseType(typeof(ResultEvent), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Adicionar([FromBody] AdicionarTipoDocumentoGrupoQuery query)
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
        public async Task<IActionResult> Atualizar([FromBody] AtualizarTipoDocumentoGrupoQuery query)
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
        [ProducesResponseType(typeof(List<TipoDocumentoGrupoResponse>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> ObterTodosUsuarioPorClienteId([FromBody] ObterTodosTipoDocumentoGrupoQuery query)
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
        [ProducesResponseType(typeof(List<TipoDocumentoGrupoResponse>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> ObterTodosTipoDocumentoGrupoCompleto()
        {
            try
            {
                var response = await _mediator.Send(new ObterTodosTipoDocumentoGrupoCompletoQuery());
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
                var response = await _mediator.Send(new DeletarTipoDocumentoGrupoPorIdQuery(id));
                return Ok(response);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType(typeof(TipoDocumentoGrupoResponse), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> ObterPorId(Guid id)
        {
            try
            {
                var response = await _mediator.Send(new ObterTipoDocumentoGrupoPorIdQuery(id));
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
        public async Task<IActionResult> AssociarTipoDocumento([FromBody] AssociarTipoDocumentoTipoDocumentoGrupoQuery query)
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
        [Route("obterTodosPorUsuarioLogado")]
        [ProducesResponseType(typeof(List<TipoDocumentoResponse>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> ObterTodosCompletoPorUsuarioLogado()
        {
            try
            {
                var response = await _mediator.Send(new ObterTodosTipoDocumentoGrupoPorUsuarioLogadoQuery());
                return Ok(response);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
    }
}
