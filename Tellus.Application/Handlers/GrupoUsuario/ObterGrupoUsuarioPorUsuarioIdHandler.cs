using Tellus.Application.Core;
using Tellus.Application.Mapper;
using Tellus.Application.Queries;
using Tellus.Application.Responses;
using Tellus.Core.Events;
using Tellus.Domain.Interfaces;
using MediatR;

namespace Tellus.Application.Handlers
{
    public class ObterGrupoUsuarioPorUsuarioIdHandler : IRequestHandler<ObterGrupoUsuarioPorUsuarioIdQuery, IEvent>
    {
        private readonly IGrupoUsuarioRepository _repository;
        private readonly IClienteRepository _repositoryCliente;
        private readonly UsuarioAutenticado _usuarioAutenticado;

        public ObterGrupoUsuarioPorUsuarioIdHandler(IGrupoUsuarioRepository repository, UsuarioAutenticado usuarioAutenticado, IClienteRepository repositoryCliente)
        {
            _repository = repository;
            _repositoryCliente = repositoryCliente;
            _usuarioAutenticado = usuarioAutenticado;
        }
        public async Task<IEvent> Handle(ObterGrupoUsuarioPorUsuarioIdQuery request, CancellationToken cancellationToken)
        {
            bool success = false;
            var response = new List<GrupoUsuarioResponse>();
            try
            {
                if (_usuarioAutenticado.Email == null)
                    return new ResultEvent(success, "Acesso expirado");

                var cliente = await _repositoryCliente.ObterPorEmailCadastroAtivo(_usuarioAutenticado.Email, cancellationToken);
                if (cliente != null)
                {
                    var lista = _repository.ObterTodosPorUsuarioId(request.Id, cliente.Id);
                    if (lista != null)
                        response = GrupoUsuarioMapper<List<GrupoUsuarioResponse>>.Map(lista);
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
