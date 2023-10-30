using Tellus.Application.Core;
using Tellus.Application.Queries;
using Tellus.Core.Events;
using Tellus.Domain.Interfaces;
using Tellus.Domain.Models;
using MediatR;
using Newtonsoft.Json;

namespace Tellus.Application.Handlers
{
    public class AssociarUsuarioGrupoUsuarioHandler : IRequestHandler<AssociarUsuarioGrupoUsuarioQuery, IEvent>
    {
        private readonly IUsuarioRepository _repositor;
        private readonly IGrupoUsuarioRepository _repositoryGrupoUsuario;
        private readonly IClienteRepository _repositoryCliente;
        private readonly UsuarioAutenticado _usuarioAutenticado;
        private readonly ILogClienteRepository _logClienteRepository;
        public AssociarUsuarioGrupoUsuarioHandler(IUsuarioRepository repository, IGrupoUsuarioRepository repositoryGrupoUsuario, UsuarioAutenticado usuarioAutenticado, IClienteRepository repositoryCliente, ILogClienteRepository logClienteRepository)
        {
            _repositor = repository;
            _repositoryGrupoUsuario = repositoryGrupoUsuario;
            _repositoryCliente = repositoryCliente;
            _usuarioAutenticado = usuarioAutenticado;
            _logClienteRepository = logClienteRepository;
        }
        public async Task<IEvent> Handle(AssociarUsuarioGrupoUsuarioQuery request, CancellationToken cancellationToken)
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
                    var usuario = await _repositor.ObterPorId(request.Id, cancellationToken);
                    if (usuario != null && usuario.ClienteId == cliente.Id)
                    {
                        var grupos = _repositoryGrupoUsuario.ObterTodosPorUsuarioId(usuario.Id, usuario.ClienteId);
                        var gruposParaDesassociar = grupos.Where(x => !request.GrupoUsuarioIds.Any(g => g == x.Id));
                        foreach (var grupo in gruposParaDesassociar)
                        {
                            grupo.UsuarioIds = grupo.UsuarioIds.Where(x => x != usuario.Id).ToList();
                            var result = await _repositoryGrupoUsuario.Salvar(grupo, cancellationToken);
                            success = true;
                        }

                        var gruposParaAssociar = request.GrupoUsuarioIds.Where(g => !grupos.Any(x => x.Id == g));
                        var gruposUsuarios = new List<string>();
                        foreach (var grupoId in gruposParaAssociar)
                        {
                            var grupo = await _repositoryGrupoUsuario.ObterPorId(grupoId, usuario.ClienteId, cancellationToken);
                            if (grupo != null)
                            {
                                gruposUsuarios.Add(grupo.Nome);
                                if (grupo.UsuarioIds == null) grupo.UsuarioIds = new List<Guid>();
                                if (!grupo.UsuarioIds.Exists(x => x == usuario.Id))
                                {
                                    grupo.UsuarioIds.Add(usuario.Id);
                                    var result = await _repositoryGrupoUsuario.Salvar(grupo, cancellationToken);
                                    success = true;
                                }
                            }
                        }
                        mensagem = success ? "Associação com sucesso!" : "Nenhuma atualização realizada.";
                        if (success)
                            await _logClienteRepository.Salvar(new LogCliente() { Id = Guid.NewGuid(), Pagina = "Associação do Usuário x Grupo de Usuário", ClienteId = cliente.Id, Acao = "Usuário: " + usuario.Nome + "<br/>Grupo de Usuários: <br/>" + JsonConvert.SerializeObject(gruposUsuarios), DataRegistro = DateTime.UtcNow }, cancellationToken);
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
