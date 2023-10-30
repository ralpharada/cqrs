using Tellus.Application.Queries;
using Tellus.Application.Responses;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Tellus.API.Controller.User
{
    [Route("api/[controller]")]
    [ApiController]
    public class LogController : ControllerBase
    {
        private readonly IMediator _mediator;
        public LogController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpPost]
        [Route("listar")]
        [ProducesResponseType(typeof(IndiceResponse), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Listar([FromBody] ListarLogQuery query)
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
