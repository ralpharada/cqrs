using Tellus.Application.Core;
using Tellus.Application.Queries;
using Tellus.Core.Events;
using Tellus.Domain.Interfaces;
using Tellus.Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Tellus.Application.Handlers
{
    public class GerarLogDocumentoHandler : IRequestHandler<GerarLogDocumentoQuery, IEvent>
    {
        private readonly ILogDocumentoRepository _repositoryLogDocumento;
        private readonly IUsuarioRepository _repositoryUsuario;
        private readonly UsuarioAutenticado _usuarioAutenticado;
        private readonly IClienteRepository _repositoryCliente;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMediator _mediator;

        public GerarLogDocumentoHandler(ILogDocumentoRepository repositoryLogDocumento,
            UsuarioAutenticado usuarioAutenticado,
            IUsuarioRepository repositoryUsuario,
            IClienteRepository repositoryCliente,
            IHttpContextAccessor httpContextAccessor,
        IMediator mediator)
        {
            _repositoryLogDocumento = repositoryLogDocumento;
            _repositoryUsuario = repositoryUsuario;
            _repositoryCliente = repositoryCliente;
            _usuarioAutenticado = usuarioAutenticado;
            _httpContextAccessor = httpContextAccessor;
            _mediator = mediator;
        }
        public async Task<IEvent> Handle(GerarLogDocumentoQuery request, CancellationToken cancellationToken)
        {
            bool success = false;
            try
            {
                if (_usuarioAutenticado.Email == null)
                    return new ResultEvent(success, "Acesso expirado");

                var usuario = await _repositoryUsuario.ObterPorEmailCadastroAtivo(_usuarioAutenticado.Email, cancellationToken);
                if (usuario != null)
                {
                    var ip = _httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();
                    await _mediator.Send(new AtualizarUsuarioLogadoQuery(usuario.Id, ip), cancellationToken);

                    var cliente = await _repositoryCliente.ObterPorId(usuario.ClienteId, cancellationToken);
                    if (cliente != null && cliente.Status)
                    {
                        LogDocumento logDocumento = new()
                        {
                            Id = Guid.NewGuid(),
                            ClienteId = usuario.ClienteId,
                            DocumentoId = request.Id,
                            DataRegistro = DateTime.UtcNow,
                            Acao = request.Acao
                        };
                        var result = await _repositoryLogDocumento.Salvar(logDocumento, cancellationToken);
                        success = result.UpsertedId > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                return new ResultEvent(success, ex.Message);
            }
            return new ResultEvent(success, null);
        }

    }
}
