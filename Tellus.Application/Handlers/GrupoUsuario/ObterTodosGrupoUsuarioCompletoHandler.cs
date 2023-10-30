using Tellus.Application.Core;
using Tellus.Application.Mapper;
using Tellus.Application.Queries;
using Tellus.Application.Responses;
using Tellus.Core.Events;
using Tellus.Domain.Interfaces;
using MediatR;

namespace Tellus.Application.Handlers
{
    public class ObterTodosGrupoUsuarioCompletoHandler : IRequestHandler<ObterTodosGrupoUsuarioCompletoQuery, IEvent>
    {
        private readonly IGrupoUsuarioRepository _repository;
        private readonly IClienteRepository _repositoryCliente;
        private readonly UsuarioAutenticado _usuarioAutenticado;

        public ObterTodosGrupoUsuarioCompletoHandler(IGrupoUsuarioRepository repository, UsuarioAutenticado usuarioAutenticado, IClienteRepository repositoryCliente)
        {
            _repository = repository;
            _repositoryCliente = repositoryCliente;
            _usuarioAutenticado = usuarioAutenticado;
        }

        public async Task<IEvent> Handle(ObterTodosGrupoUsuarioCompletoQuery request, CancellationToken cancellationToken)
        {
            bool success = false;
            var response = new List<GrupoUsuarioResponse>();
            long total = 0;
            try
            {
                if (_usuarioAutenticado.Email == null)
                    return new ResultEvent(success, "Acesso expirado");

                var cliente = await _repositoryCliente.ObterPorEmailCadastroAtivo(_usuarioAutenticado.Email, cancellationToken);
                if (cliente != null)
                {
                    var taskResult = await _repository.ObterTodos(cliente.Id, cancellationToken);
                    total = await _repository.Count(cliente.Id, cancellationToken);
                    response = GrupoUsuarioMapper<List<GrupoUsuarioResponse>>.Map(taskResult);
                    success = true;
                }
            }
            catch (Exception ex)
            {
                return new ResultEvent(success, ex.Message);
            }
            return new ResultEvent(success, response, total);
        }

    }
}
