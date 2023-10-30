using Tellus.Application.Core;
using Tellus.Application.Mapper;
using Tellus.Application.Queries;
using Tellus.Application.Responses;
using Tellus.Core.Events;
using Tellus.Domain.Interfaces;
using MediatR;

namespace Tellus.Application.Handlers
{
    public class ObterTodosPermissaoHandler : IRequestHandler<ObterTodosPermissaoQuery, IEvent>
    {
        private readonly IPermissaoRepository _repository;
        private readonly IClienteRepository _repositoryCliente;
        private readonly UsuarioAutenticado _usuarioAutenticado;

        public ObterTodosPermissaoHandler(IPermissaoRepository repository, UsuarioAutenticado usuarioAutenticado, IClienteRepository repositoryCliente)
        {
            _repository = repository;
            _repositoryCliente = repositoryCliente;
            _usuarioAutenticado = usuarioAutenticado;
        }
        public async Task<IEvent> Handle(ObterTodosPermissaoQuery request, CancellationToken cancellationToken)
        {
            bool success = false;
            var response = new List<PermissaoResponse>();
            try
            {
                if (_usuarioAutenticado.Email == null)
                    return new ResultEvent(success, "Acesso expirado");

                var cliente = await _repositoryCliente.ObterPorEmailCadastroAtivo(_usuarioAutenticado.Email, cancellationToken);
                if (cliente != null)
                {
                    var lista = await _repository.ObterTodos(request.VinculoId, cancellationToken);
                    response = PermissaoMapper<List<PermissaoResponse>>.Map(lista);
                    success = true;
                }
            }
            catch (Exception ex)
            {
                return new ResultEvent(success, ex.Message);
            }
            return new ResultEvent(success, response, null);
        }

    }
}
