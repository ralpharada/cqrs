using Tellus.Application.Core;
using Tellus.Application.Mapper;
using Tellus.Application.Queries;
using Tellus.Application.Responses;
using Tellus.Core.Events;
using Tellus.Domain.Interfaces;
using MediatR;

namespace Tellus.Application.Handlers
{
    public class ObterTodosVinculoHandler : IRequestHandler<ObterTodosVinculoQuery, IEvent>
    {
        private readonly IVinculoRepository _repository;
        private readonly IUsuarioAdmRepository _repositoryUsuarioAdm;
        private readonly UsuarioAutenticado _usuarioAutenticado;

        public ObterTodosVinculoHandler(IVinculoRepository repository, UsuarioAutenticado usuarioAutenticado, IUsuarioAdmRepository repositoryUsuarioAdm)
        {
            _repository = repository;
            _repositoryUsuarioAdm = repositoryUsuarioAdm;
            _usuarioAutenticado = usuarioAutenticado;
        }
        public async Task<IEvent> Handle(ObterTodosVinculoQuery request, CancellationToken cancellationToken)
        {
            bool success = false;
            var response = new List<VinculoResponse>();
            try
            {
                if (_usuarioAutenticado.Email == null)
                    return new ResultEvent(success, "Acesso expirado");

                var usuarioAdm = await _repositoryUsuarioAdm.ObterPorEmailCadastroAtivo(_usuarioAutenticado.Email, cancellationToken);
                if (usuarioAdm != null)
                {
                    var lista = await _repository.ObterTodos(cancellationToken);
                    response = VinculoMapper<List<VinculoResponse>>.Map(lista);
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
