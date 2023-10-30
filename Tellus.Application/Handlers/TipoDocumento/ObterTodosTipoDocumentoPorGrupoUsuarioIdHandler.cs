using Tellus.Application.Mapper;
using Tellus.Application.Queries;
using Tellus.Application.Responses;
using Tellus.Core.Events;
using Tellus.Domain.Interfaces;
using MediatR;

namespace Tellus.Application.Handlers
{
    public class ObterTodosTipoDocumentoPorGrupoUsuarioIdHandler : IRequestHandler<ObterTodosTipoDocumentoPorGrupoUsuarioIdQuery, IEvent>
    {
        private readonly ITipoDocumentoRepository _repository;
        private readonly IGrupoUsuarioRepository _repositoryGrupoUsuario;

        public ObterTodosTipoDocumentoPorGrupoUsuarioIdHandler(ITipoDocumentoRepository repository, IGrupoUsuarioRepository repositoryGrupoUsuario)
        {
            _repository = repository;
            _repositoryGrupoUsuario = repositoryGrupoUsuario;
        }
        public async Task<IEvent> Handle(ObterTodosTipoDocumentoPorGrupoUsuarioIdQuery request, CancellationToken cancellationToken)
        {
            bool success = false;
            var response = new List<TipoDocumentoResponse>();
            try
            {
                var grupoUsuario = await _repositoryGrupoUsuario.ObterPorId(request.GrupoUsuarioId, request.ClienteId, cancellationToken);

                if (grupoUsuario != null)
                {
                    if (grupoUsuario.TipoDocumentoIds != null)
                    {
                        var lista = _repository.ObterTodosPorIds(grupoUsuario.TipoDocumentoIds, request.ClienteId);
                        response = TipoDocumentoMapper<List<TipoDocumentoResponse>>.Map(lista);
                    }
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
