using Tellus.Application.Core;
using Tellus.Application.Mapper;
using Tellus.Application.Queries;
using Tellus.Application.Responses;
using Tellus.Core.Events;
using Tellus.Domain.Interfaces;
using Tellus.Domain.Models;
using MediatR;

namespace Tellus.Application.Handlers
{
    public class ObterTodosTipoDocumentoGrupoPorUsuarioLogadoHandler : IRequestHandler<ObterTodosTipoDocumentoGrupoPorUsuarioLogadoQuery, IEvent>
    {
        private readonly ITipoDocumentoGrupoRepository _repository;
        private readonly ITipoDocumentoRepository _repositoryTipoDocumento;
        private readonly IUsuarioRepository _repositoryUsuario;
        private readonly IClienteRepository _repositoryCliente;
        private readonly UsuarioAutenticado _usuarioAutenticado;

        public ObterTodosTipoDocumentoGrupoPorUsuarioLogadoHandler(ITipoDocumentoGrupoRepository repository, UsuarioAutenticado usuarioAutenticado, IClienteRepository repositoryCliente, IUsuarioRepository repositoryUsuario, ITipoDocumentoRepository repositoryTipoDocumento)
        {
            _repository = repository;
            _repositoryUsuario = repositoryUsuario;
            _repositoryCliente = repositoryCliente;
            _usuarioAutenticado = usuarioAutenticado;
            _repositoryTipoDocumento = repositoryTipoDocumento;
        }

        public async Task<IEvent> Handle(ObterTodosTipoDocumentoGrupoPorUsuarioLogadoQuery request, CancellationToken cancellationToken)
        {
            bool success = false;
            var response = new List<TipoDocumentoGrupoResponse>();
            try
            {
                if (_usuarioAutenticado.Email == null)
                    return new ResultEvent(success, "Acesso expirado");
                var usuario = await _repositoryUsuario.ObterPorEmailCadastroAtivo(_usuarioAutenticado.Email, cancellationToken);
                if (usuario != null)
                {
                    var cliente = await _repositoryCliente.ObterPorId(usuario.ClienteId, cancellationToken);
                    if (cliente != null)
                    {
                        var taskResult = await _repository.ObterTodosCompletoAtivo(cliente.Id, cancellationToken);
                        foreach (var tipoDocumento in taskResult)
                        {
                            tipoDocumento.TipoDocumentos = new List<TipoDocumento>();
                            if (tipoDocumento.TipoDocumentoIds != null)
                                tipoDocumento.TipoDocumentos = _repositoryTipoDocumento.ObterTodosPorIds(tipoDocumento.TipoDocumentoIds, usuario.ClienteId);
                        }
                        response = TipoDocumentoGrupoMapper<List<TipoDocumentoGrupoResponse>>.Map(taskResult);
                        success = true;
                    }
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
