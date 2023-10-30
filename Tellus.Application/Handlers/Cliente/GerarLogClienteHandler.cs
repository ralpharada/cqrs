using Tellus.Application.Core;
using Tellus.Application.Queries;
using Tellus.Core.Events;
using Tellus.Domain.Interfaces;
using Tellus.Domain.Models;
using MediatR;

namespace Tellus.Application.Handlers
{
    public class GerarLogClienteHandler : IRequestHandler<GerarLogClienteQuery, IEvent>
    {
        private readonly ILogClienteRepository _repositoryLogCliente;
        private readonly IUsuarioRepository _repositoryUsuario;
        private readonly UsuarioAutenticado _usuarioAutenticado;
        private readonly IClienteRepository _repositoryCliente;

        public GerarLogClienteHandler(ILogClienteRepository repositoryLogCliente, UsuarioAutenticado usuarioAutenticado, IUsuarioRepository repositoryUsuario, IClienteRepository repositoryCliente)
        {
            _repositoryLogCliente = repositoryLogCliente;
            _repositoryUsuario = repositoryUsuario;
            _repositoryCliente = repositoryCliente;
            _usuarioAutenticado = usuarioAutenticado;
        }
        public async Task<IEvent> Handle(GerarLogClienteQuery request, CancellationToken cancellationToken)
        {
            bool success = false;
            try
            {
                if (_usuarioAutenticado.Email == null)
                    return new ResultEvent(success, "Acesso expirado");

                var cliente = await _repositoryCliente.ObterPorEmailCadastroAtivo(_usuarioAutenticado.Email, cancellationToken);
                if (cliente != null && cliente.Status)
                {
                    LogCliente logCliente = new()
                    {
                        Id = Guid.NewGuid(),
                        ClienteId = cliente.Id,
                        Pagina = request.Pagina,
                        DataRegistro = DateTime.UtcNow,
                        Acao = request.Acao
                    };
                    var result = await _repositoryLogCliente.Salvar(logCliente, cancellationToken);
                    success = result.UpsertedId > 0;
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
