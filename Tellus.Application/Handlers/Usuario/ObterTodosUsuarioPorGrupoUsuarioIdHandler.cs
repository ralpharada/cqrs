using Tellus.Application.Core;
using Tellus.Application.Mapper;
using Tellus.Application.Queries;
using Tellus.Application.Responses;
using Tellus.Core.Events;
using Tellus.Domain.Interfaces;
using MediatR;

namespace Tellus.Application.Handlers
{
    public class ObterTodosUsuarioPorGrupoUsuarioIdHandler : IRequestHandler<ObterTodosUsuarioPorGrupoUsuarioIdQuery, IEvent>
    {
        private readonly IUsuarioRepository _repository;
        private readonly IGrupoUsuarioRepository _repositoryGrupoUsuario;
        private readonly IClienteRepository _repositoryCliente;
        private readonly UsuarioAutenticado _usuarioAutenticado;

        public ObterTodosUsuarioPorGrupoUsuarioIdHandler(IUsuarioRepository repository, IClienteRepository repositoryCliente, UsuarioAutenticado usuarioAutenticado, IGrupoUsuarioRepository repositoryGrupoUsuario)
        {
            _repository = repository;
            _repositoryGrupoUsuario = repositoryGrupoUsuario;
            _repositoryCliente = repositoryCliente;
            _usuarioAutenticado = usuarioAutenticado;
        }

        public async Task<IEvent> Handle(ObterTodosUsuarioPorGrupoUsuarioIdQuery request, CancellationToken cancellationToken)
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
                    var grupoUsuario = await _repositoryGrupoUsuario.ObterPorId(request.GrupoUsuarioId, cliente.Id, cancellationToken);

                    if (grupoUsuario != null)
                    {
                        if (grupoUsuario.UsuarioIds != null)
                        {
                            var lista = _repository.ObterTodosPorIds(grupoUsuario.UsuarioIds, cliente.Id);
                            response = UsuarioMapper<List<UsuarioResponse>>.Map(lista);
                        }
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
