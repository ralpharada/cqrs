using Tellus.Application.Core;
using Tellus.Application.Queries;
using Tellus.Core.Events;
using Tellus.Domain.Interfaces;
using Tellus.Domain.Models;
using MediatR;
using Newtonsoft.Json;

namespace Tellus.Application.Handlers
{
    public class AssociarGrupoUsuarioTipoDocumentoHandler : IRequestHandler<AssociarGrupoUsuarioTipoDocumentoQuery, IEvent>
    {
        private readonly IGrupoUsuarioRepository _repositoryGrupoUsuario;
        private readonly IClienteRepository _repositoryCliente;
        private readonly UsuarioAutenticado _usuarioAutenticado;
        private readonly ILogClienteRepository _logClienteRepository;
        private readonly ITipoDocumentoRepository _tipoDocumentoRepository;

        public AssociarGrupoUsuarioTipoDocumentoHandler(IGrupoUsuarioRepository repositoryGrupoUsuario, UsuarioAutenticado usuarioAutenticado, IClienteRepository repositoryCliente, ILogClienteRepository logClienteRepository, ITipoDocumentoRepository tipoDocumentoRepository)
        {
            _repositoryGrupoUsuario = repositoryGrupoUsuario;
            _repositoryCliente = repositoryCliente;
            _usuarioAutenticado = usuarioAutenticado;
            _logClienteRepository = logClienteRepository;
            _tipoDocumentoRepository = tipoDocumentoRepository;
        }
        public async Task<IEvent> Handle(AssociarGrupoUsuarioTipoDocumentoQuery request, CancellationToken cancellationToken)
        {
            string mensagem = "";
            bool success = false;
            try
            {
                if (_usuarioAutenticado.Email == null)
                    return new ResultEvent(success, "Acesso expirado");

                var cliente = await _repositoryCliente.ObterPorEmailCadastroAtivo(_usuarioAutenticado.Email, cancellationToken);
                if (cliente != null)
                {
                    var grupoUsuario = await _repositoryGrupoUsuario.ObterPorId(request.Id, cliente.Id, cancellationToken);
                    if (grupoUsuario != null)
                    {
                        grupoUsuario.TipoDocumentoIds = request.TipoDocumentoIds;
                        var tiposDocumentos = _tipoDocumentoRepository.ObterTodosPorIds(grupoUsuario.TipoDocumentoIds, cliente.Id);
                        if (grupoUsuario.VinculoPermissoes != null)
                        {
                            var itemsToRemove = new List<VinculoPermissao>();
                            foreach (var item in grupoUsuario.VinculoPermissoes)
                            {
                                if (!grupoUsuario.TipoDocumentoIds.Exists(x => x == item.TipoDocumentoId))
                                {
                                    itemsToRemove.Add(item);
                                }
                            }
                            foreach (var itemToRemove in itemsToRemove)
                            {
                                grupoUsuario.VinculoPermissoes.Remove(itemToRemove);
                            }
                        }
                        var resultTipoDocumento = await _repositoryGrupoUsuario.Salvar(grupoUsuario, cancellationToken);
                        success = resultTipoDocumento.ModifiedCount > 0;
                        mensagem = success ? "Associação com sucesso!" : "Nenhuma atualização realizada.";
                        if (success)
                            await _logClienteRepository.Salvar(new LogCliente() { Id = Guid.NewGuid(), Pagina = "Associação do Grupo de Usuário x Tipo Documento", ClienteId = cliente.Id, Acao = "Grupo de Usuários: " + grupoUsuario.Nome + "<br/>Tipo Documento: <br/>" + JsonConvert.SerializeObject(tiposDocumentos.Select(x => x.Nome)), DataRegistro = DateTime.UtcNow }, cancellationToken);
                    }
                }
            }
            catch (Exception ex)
            {
                mensagem = ex.Message;
            }
            return new ResultEvent(success, mensagem);
        }

    }
}
