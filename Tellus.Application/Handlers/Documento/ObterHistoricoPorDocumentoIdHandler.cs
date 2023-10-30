using Tellus.Application.Core;
using Tellus.Application.Queries;
using Tellus.Core.Events;
using Tellus.Domain.Interfaces;
using Tellus.Domain.Models;
using MediatR;

namespace Tellus.Application.Handlers
{
    public class ObterHistoricoPorDocumentoIdHandler : IRequestHandler<ObterHistoricoPorDocumentoIdQuery, IEvent>
    {
        private readonly ILogDocumentoRepository _repository;
        private readonly IUsuarioRepository _repositoryUsuario;
        private readonly UsuarioAutenticado _usuarioAutenticado;
        private readonly IClienteRepository _repositoryCliente;

        public ObterHistoricoPorDocumentoIdHandler( ILogDocumentoRepository repository, UsuarioAutenticado usuarioAutenticado, IUsuarioRepository repositoryUsuario, IClienteRepository repositoryCliente)
        {
            _repository = repository;
            _repositoryUsuario = repositoryUsuario;
            _usuarioAutenticado = usuarioAutenticado;
            _repositoryCliente = repositoryCliente;
        }
        public async Task<IEvent> Handle(ObterHistoricoPorDocumentoIdQuery request, CancellationToken cancellationToken)
        {
            bool success = false;
            var response = new List<LogDocumento>();
            try
            {
                if (_usuarioAutenticado.Email == null)
                    return new ResultEvent(success, "Acesso expirado");

                var usuario = await _repositoryUsuario.ObterPorEmailCadastroAtivo(_usuarioAutenticado.Email, cancellationToken);
                if (usuario != null)
                {
                    var cliente = await _repositoryCliente.ObterPorId(usuario.ClienteId, cancellationToken);
                    if (cliente != null && cliente.Status)
                    {
                        response = await _repository.ObterTodosPorDocumentoId(request.Id, usuario.ClienteId, cancellationToken);
                        success = true;
                    }
                }
            }
            catch (Exception ex)
            {
                return new ResultEvent(success, ex.Message);
            }
            return new ResultEvent(success, response);
        }

    }
}
