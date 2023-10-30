using Tellus.Application.Core;
using Tellus.Application.Queries;
using Tellus.Core.Events;
using Tellus.Domain.Interfaces;
using Tellus.Domain.Models;
using MediatR;
using Newtonsoft.Json;

namespace Tellus.Application.Handlers
{
    public class AssociarGrupoUsuarioPermissaoHandler : IRequestHandler<AssociarGrupoUsuarioPermissaoQuery, IEvent>
    {
        private readonly IGrupoUsuarioRepository _repositoryGrupoUsuario;
        private readonly IClienteRepository _repositoryCliente;
        private readonly UsuarioAutenticado _usuarioAutenticado;
        private readonly ILogClienteRepository _logClienteRepository;
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IPermissaoRepository _permissaoRepository;
        public AssociarGrupoUsuarioPermissaoHandler(IGrupoUsuarioRepository repositoryGrupoUsuario, UsuarioAutenticado usuarioAutenticado, IClienteRepository repositoryCliente, ILogClienteRepository logClienteRepository, IUsuarioRepository usuarioRepository, IPermissaoRepository permissaoRepository)
        {
            _repositoryGrupoUsuario = repositoryGrupoUsuario;
            _repositoryCliente = repositoryCliente;
            _usuarioAutenticado = usuarioAutenticado;
            _logClienteRepository = logClienteRepository;
            _usuarioRepository = usuarioRepository;
            _permissaoRepository = permissaoRepository;
        }
        public async Task<IEvent> Handle(AssociarGrupoUsuarioPermissaoQuery request, CancellationToken cancellationToken)
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
                        grupoUsuario.PermissaoIds = request.PermissaoIds;
                        var permissoes = _permissaoRepository.ObterTodosPorIds(grupoUsuario.PermissaoIds);
                        var resultTipoDocumento = await _repositoryGrupoUsuario.Salvar(grupoUsuario, cancellationToken);
                        success = resultTipoDocumento.ModifiedCount > 0;
                        mensagem = success ? "Associação com sucesso!" : "Nenhuma atualização realizada.";
                        if (success)
                            await _logClienteRepository.Salvar(new LogCliente() { Id = Guid.NewGuid(), Pagina = "Associação do Grupo de Usuário x Permissão", ClienteId = cliente.Id, Acao = "Grupo de Usuário: " + grupoUsuario.Nome + "<br/>Permissão: <br/>" + JsonConvert.SerializeObject(permissoes.Select(x => x.Nome)), DataRegistro = DateTime.UtcNow }, cancellationToken);
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
