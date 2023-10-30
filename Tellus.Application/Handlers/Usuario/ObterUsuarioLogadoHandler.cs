using Tellus.Application.Core;
using Tellus.Application.Mapper;
using Tellus.Application.Queries;
using Tellus.Application.Responses;
using Tellus.Core.Events;
using Tellus.Domain.Interfaces;
using MediatR;

namespace Tellus.Application.Handlers
{
    public class ObterUsuarioLogadoHandler : IRequestHandler<ObterUsuarioLogadoQuery, IEvent>
    {
        private readonly IUsuarioRepository _repository;
        private readonly UsuarioAutenticado _usuarioAutenticado;
        private readonly IGrupoUsuarioRepository _repositoryGrupoUsuario;
        private readonly IClienteRepository _repositoryCliente;

        public ObterUsuarioLogadoHandler(IUsuarioRepository repository, UsuarioAutenticado usuarioAutenticado, IGrupoUsuarioRepository repositoryGrupoUsuario, IClienteRepository repositoryCliente)
        {
            _repository = repository;
            _usuarioAutenticado = usuarioAutenticado;
            _repositoryGrupoUsuario = repositoryGrupoUsuario;
            _repositoryCliente = repositoryCliente;
        }

        public async Task<IEvent> Handle(ObterUsuarioLogadoQuery request, CancellationToken cancellationToken)
        {
            var success = false;
            var response = new UsuarioLogadoResponse();
            try
            {
                if (_usuarioAutenticado.Email == null)
                    return new ResultEvent(success, "Acesso expirado");

                var usuario = await _repository.ObterPorEmailCadastroAtivo(_usuarioAutenticado.Email, cancellationToken);
                if (usuario != null)
                {
                    var cliente = await _repositoryCliente.ObterPorId(usuario.ClienteId, cancellationToken);
                    if (cliente != null && cliente.Status)
                    {
                        var grupos = _repositoryGrupoUsuario.ObterTodosPorUsuarioId(usuario.Id, usuario.ClienteId);
                        var grupoResponse = GrupoUsuarioMapper<List<GrupoUsuarioPermissaoResponse>>.Map(grupos);
                        response = new UsuarioLogadoResponse()
                        {
                            UsuarioNome = usuario.Nome,
                            Permissoes = usuario.PermissaoIds,
                            TipoDocumentoIds = usuario.TipoDocumentoIds,
                            VinculoPermissao = VinculoPermissaoMapper<List<VinculoPermissaoResumidoResponse>>.Map(usuario.VinculoPermissoes),
                            Grupos = grupoResponse,
                            Produtos = usuario.ProdutoIds,
                            Chave = cliente.Chave
                        };
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
