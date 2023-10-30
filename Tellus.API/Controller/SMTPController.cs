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
    public class SMTPController : ControllerBase
    {
        private readonly IMediator _mediator;
        public SMTPController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpPost]
        [ProducesResponseType(typeof(ResultEvent), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Adicionar([FromBody] AdicionarSMTPQuery query)
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
        public async Task<IActionResult> Atualizar([FromBody] AtualizarSMTPQuery query)
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
        [ProducesResponseType(typeof(List<SMTPResponse>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> ObterTodosUsuarioPorClienteId([FromBody] ObterTodosSMTPPorClienteIdQuery query)
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
                var response = await _mediator.Send(new DeletarSMTPPorIdQuery(id));
                return Ok(response);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType(typeof(SMTPResponse), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> ObterPorId(Guid id)
        {
            try
            {
                var response = await _mediator.Send(new ObterSMTPPorIdQuery(id));
                return Ok(response);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
    }
}
