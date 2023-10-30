using Tellus.Application.Core;
using Tellus.Application.Queries;
using Tellus.Application.Responses;
using Tellus.Core.Events;
using Tellus.Domain.Interfaces;
using MediatR;

namespace Tellus.Application.Handlers
{
    public class RelatorioHandler : IRequestHandler<RelatorioQuery, IEvent>
    {
        private readonly IUsuarioRepository _repository;
        private readonly IDocumentoRepository _repositoryDocumento;
        private readonly IClienteRepository _repositoryCliente;
        private readonly IGrupoUsuarioRepository _repositoryGrupoUsuario;
        private readonly ITipoDocumentoRepository _repositoryTipoDocumento;
        private readonly UsuarioAutenticado _usuarioAutenticado;

        public RelatorioHandler(IUsuarioRepository repository, IClienteRepository repositoryCliente, IGrupoUsuarioRepository repositoryGrupoUsuario, ITipoDocumentoRepository repositoryTipoDocumento, UsuarioAutenticado usuarioAutenticado, IDocumentoRepository repositoryDocumento)
        {
            _repository = repository;
            _repositoryDocumento = repositoryDocumento;
            _repositoryCliente = repositoryCliente;
            _usuarioAutenticado = usuarioAutenticado;
            _repositoryGrupoUsuario = repositoryGrupoUsuario;
            _repositoryTipoDocumento = repositoryTipoDocumento;
        }

        public async Task<IEvent> Handle(RelatorioQuery request, CancellationToken cancellationToken)
        {
            var success = false;
            var response = new List<RelatorioResponse>();
            try
            {
                if (_usuarioAutenticado.Email == null)
                    return new ResultEvent(success, "Acesso expirado");

                var cliente = await _repositoryCliente.ObterPorEmailCadastroAtivo(_usuarioAutenticado.Email, cancellationToken);
                if (cliente != null)
                {
                    var listaUsuarios = await _repository.ObterTodosCompleto(cliente.Id, cancellationToken);
                    int usuariosAtivos = listaUsuarios.Count(x => x.Status);
                    int usuariosInativos = listaUsuarios.Count(x => !x.Status);
                    response.Add(new RelatorioResponse()
                    {
                        Titulo = "Usuários",
                        Json = $"{{\"Ativos\":{usuariosAtivos},\"Inativos\":{usuariosInativos},\"Cadastrados\":{listaUsuarios.Count()}}}"
                    });
                    var listaGruposUsuarios = await _repositoryGrupoUsuario.ObterTodos(cliente.Id, cancellationToken);
                    int gruposUsuariosAtivos = listaGruposUsuarios.Count(x => x.Status);
                    int gruposUsuariosInativos = listaGruposUsuarios.Count(x => !x.Status);
                    response.Add(new RelatorioResponse()
                    {
                        Titulo = "Grupos de Usuários",
                        Json = $"{{\"Ativos\":{gruposUsuariosAtivos},\"Inativos\":{gruposUsuariosInativos}}}"
                    });
                    var listaTiposDocumentos = await _repositoryTipoDocumento.ObterTodosCompleto(cliente.Id, cancellationToken);
                    int tiosDocumentosAtivos = listaTiposDocumentos.Count(x => x.Status);
                    int tiosDocumentosInativos = listaTiposDocumentos.Count(x => !x.Status);
                    response.Add(new RelatorioResponse()
                    {
                        Titulo = "Tipos de Documentos",
                        Json = $"{{\"Ativos\":{tiosDocumentosAtivos},\"Inativos\":{tiosDocumentosInativos}}}"
                    });
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
