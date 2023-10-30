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
    public class IndiceController : ControllerBase
    {
        private readonly IMediator _mediator;
        public IndiceController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpPost]
        [ProducesResponseType(typeof(ResultEvent), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Adicionar([FromBody] AdicionarIndiceQuery query)
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
        public async Task<IActionResult> Atualizar([FromBody] AtualizarIndiceQuery query)
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
        [Route("obterTodosPorCliente")]
        [ProducesResponseType(typeof(List<IndiceResponse>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> ObterTodosUsuarioPorClienteId()
        {
            try
            {
                var response = await _mediator.Send(new ObterTodosIndicePorClienteIdQuery());
                return Ok(response);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
        [HttpPost]
        [Route("obterTodosPorTipoDocumento")]
        [ProducesResponseType(typeof(List<IndiceResponse>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> ObterTodosIndicePorTipoDocumentoId([FromBody] ObterTodosIndicePorTipoDocumentoIdQuery query)
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
        [ProducesResponseType(typeof(List<IndiceResponse>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> ObterTodosUsuarioLogado()
        {
            try
            {
                var response = await _mediator.Send(new ObterTodosIndicePorUsuarioLogadoQuery());
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
                var response = await _mediator.Send(new DeletarIndicePorIdQuery(id));
                return Ok(response);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType(typeof(IndiceResponse), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> ObterPorId(Guid id)
        {
            try
            {
                var response = await _mediator.Send(new ObterIndicePorIdQuery(id));
                return Ok(response);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
    }
}
