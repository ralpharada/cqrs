using Tellus.Application.Core;
using Tellus.Application.Queries;
using Tellus.Application.Responses;
using Tellus.Core.Events;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Tellus.API.Controller.Relatorio
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class RelatorioController : ControllerBase
    {
        private readonly IMediator _mediator;
        public RelatorioController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpGet]
        [Route("")]
        [ProducesResponseType(typeof(RelatorioResponse), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Relatorios()
        {
            try
            {
                var response = await _mediator.Send(new RelatorioQuery());
                return Ok(response);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
        [HttpPost]
        [Route("tipodocumentos_documentos")]
        [ProducesResponseType(typeof(RelatorioResponse), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> TipoDocumentos_Documentos([FromBody] RelatorioTipoDocumentoPorDocumentosQuery query)
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
        [Route("documentosultimos12meses")]
        [ProducesResponseType(typeof(RelatorioResponse), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> TipoDocumentos_Documentos([FromBody] RelatorioDocumentosUltimos12MesesQuery query)
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
        [Route("documentos")]
        [ProducesResponseType(typeof(RelatorioResponse), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Documentos([FromBody] RelatorioDocumentosQuery query)
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
