using Tellus.Application.Core;
using Tellus.Application.Queries;
using Tellus.Core.Events;
using Tellus.Domain.Interfaces;
using Tellus.Domain.Models;
using MediatR;
using Newtonsoft.Json;

namespace Tellus.Application.Handlers
{
    public class AssociarGrupoUsuarioVinculoPermissaoHandler : IRequestHandler<AssociarGrupoUsuarioVinculoPermissaoQuery, IEvent>
    {
        private readonly IGrupoUsuarioRepository _repositoryGrupoUsuario;
        private readonly IClienteRepository _repositoryCliente;
        private readonly UsuarioAutenticado _usuarioAutenticado;
        private readonly ILogClienteRepository _logClienteRepository;
        public AssociarGrupoUsuarioVinculoPermissaoHandler(IGrupoUsuarioRepository repositoryGrupoUsuario, UsuarioAutenticado usuarioAutenticado, IClienteRepository repositoryCliente, ILogClienteRepository logClienteRepository)
        {
            _repositoryGrupoUsuario = repositoryGrupoUsuario;
            _repositoryCliente = repositoryCliente;
            _usuarioAutenticado = usuarioAutenticado;
            _logClienteRepository = logClienteRepository;
        }
        public async Task<IEvent> Handle(AssociarGrupoUsuarioVinculoPermissaoQuery request, CancellationToken cancellationToken)
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
                    var grupoUsuario = await _repositoryGrupoUsuario.ObterPorId(request.GrupoUsuarioId, cliente.Id, cancellationToken);
                    if (grupoUsuario != null)
                    {
                        grupoUsuario.VinculoPermissoes = request.VinculoPermissao;
                        var resultTipoDocumento = await _repositoryGrupoUsuario.Salvar(grupoUsuario, cancellationToken);
                        success = resultTipoDocumento.ModifiedCount > 0;
                        mensagem = success ? "Associação com sucesso!" : "Nenhuma atualização realizada.";
                        if (success)
                            await _logClienteRepository.Salvar(new LogCliente() { Id = Guid.NewGuid(), Pagina = "Associação do Grupo de Usuário: Tipo Documento x Permissão", ClienteId = cliente.Id, Acao = "Grupo de Usuário: " + grupoUsuario.Nome + "<br/>Permissão: <br/>" + JsonConvert.SerializeObject(request.VinculoPermissao), DataRegistro = DateTime.UtcNow }, cancellationToken);
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
