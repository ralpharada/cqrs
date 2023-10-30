using Tellus.Application.Core;
using Tellus.Application.Mapper;
using Tellus.Application.Queries;
using Tellus.Application.Responses;
using Tellus.Core.Events;
using Tellus.Domain.Interfaces;
using MediatR;

namespace Tellus.Application.Handlers
{
    public class ObterTipoDocumentoGrupoPorIdHandler : IRequestHandler<ObterTipoDocumentoGrupoPorIdQuery, IEvent>
    {
        private readonly ITipoDocumentoGrupoRepository _repository;
        private readonly ITipoDocumentoRepository _repositoryTipoDocumento;
        private readonly IUsuarioRepository _repositoryUsuario;
        private readonly IClienteRepository _repositoryCliente;
        private readonly UsuarioAutenticado _usuarioAutenticado;

        public ObterTipoDocumentoGrupoPorIdHandler(ITipoDocumentoGrupoRepository repository, IUsuarioRepository repositoryUsuario, ITipoDocumentoRepository repositoryTipoDocumento, UsuarioAutenticado usuarioAutenticado, IClienteRepository repositoryCliente)
        {
            _repository = repository;
            _repositoryUsuario = repositoryUsuario;
            _repositoryTipoDocumento = repositoryTipoDocumento;
            _repositoryCliente = repositoryCliente;
            _usuarioAutenticado = usuarioAutenticado;
        }
        public async Task<IEvent> Handle(ObterTipoDocumentoGrupoPorIdQuery request, CancellationToken cancellationToken)
        {
            bool success = false;
            var response = new TipoDocumentoGrupoResponse();
            try
            {
                if (_usuarioAutenticado.Email == null)
                    return new ResultEvent(success, "Acesso expirado");

                var cliente = await _repositoryCliente.ObterPorEmailCadastroAtivo(_usuarioAutenticado.Email, cancellationToken);
                if (cliente != null)
                {
                    var tipoDocumentoGrupo = await _repository.ObterPorId(request.Id, cliente.Id, cancellationToken);
                    if (tipoDocumentoGrupo.TipoDocumentoIds != null)
                        tipoDocumentoGrupo.TipoDocumentos = _repositoryTipoDocumento.ObterTodosPorIds(tipoDocumentoGrupo.TipoDocumentoIds, cliente.Id);
                    response = TipoDocumentoGrupoMapper<TipoDocumentoGrupoResponse>.Map(tipoDocumentoGrupo);
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
