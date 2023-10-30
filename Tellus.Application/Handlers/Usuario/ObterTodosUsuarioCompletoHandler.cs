using Tellus.Application.Core;
using Tellus.Application.Mapper;
using Tellus.Application.Queries;
using Tellus.Application.Responses;
using Tellus.Core.Events;
using Tellus.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Configuration;

namespace Tellus.Application.Handlers
{
    public class ObterTodosUsuarioCompletoHandler : IRequestHandler<ObterTodosUsuarioCompletoQuery, IEvent>
    {
        private readonly IUsuarioRepository _repository;
        private readonly IClienteRepository _repositoryCliente;
        private readonly UsuarioAutenticado _usuarioAutenticado;

        public ObterTodosUsuarioCompletoHandler(IUsuarioRepository repository, IClienteRepository repositoryCliente, UsuarioAutenticado usuarioAutenticado)
        {
            _repository = repository;
            _repositoryCliente = repositoryCliente;
            _usuarioAutenticado = usuarioAutenticado;
        }

        public async Task<IEvent> Handle(ObterTodosUsuarioCompletoQuery request, CancellationToken cancellationToken)
        {
            var success = false;
            var response = new List<UsuarioResponse>();
            try
            {
                if (_usuarioAutenticado.Email == null)
                    return new ResultEvent(success, "Acesso expirado");

                var cliente = await _repositoryCliente.ObterPorEmailCadastroAtivo(_usuarioAutenticado.Email, cancellationToken);
                if (cliente != null)
                {
                    var lista = await _repository.ObterTodosCompleto(cliente.Id, cancellationToken);
                    response = UsuarioMapper<List<UsuarioResponse>>.Map(lista);
                    success = true;
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
