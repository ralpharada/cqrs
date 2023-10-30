using Tellus.Application.Core;
using Tellus.Application.Queries;
using Tellus.Core.Events;
using Tellus.Domain.Interfaces;
using Tellus.Domain.Models;
using MediatR;
using Newtonsoft.Json;

namespace Tellus.Application.Handlers
{
    public class AtualizarGrupoUsuarioHandler : IRequestHandler<AtualizarGrupoUsuarioQuery, IEvent>
    {
        private readonly IGrupoUsuarioRepository _repository;
        private readonly IClienteRepository _repositoryCliente;
        private readonly UsuarioAutenticado _usuarioAutenticado;
        private readonly ILogClienteRepository _logClienteRepository;

        public AtualizarGrupoUsuarioHandler(IGrupoUsuarioRepository repository, UsuarioAutenticado usuarioAutenticado, IClienteRepository repositoryCliente, ILogClienteRepository logClienteRepository)
        {
            _repository = repository;
            _repositoryCliente = repositoryCliente;
            _usuarioAutenticado = usuarioAutenticado;
            _logClienteRepository = logClienteRepository;
        }
        public async Task<IEvent> Handle(AtualizarGrupoUsuarioQuery request, CancellationToken cancellationToken)
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
                    var grupoUsuario = await _repository.ObterPorId(request.Id, cliente.Id, cancellationToken);
                    if (grupoUsuario != null)
                    {
                        grupoUsuario.Nome = request.Nome;
                        grupoUsuario.Status = request.Status;
                        grupoUsuario.DataUltimaAlteracao = DateTime.UtcNow;
                        var result = await _repository.Salvar(grupoUsuario, cancellationToken);
                        success = result.ModifiedCount > 0;
                        mensagem = success ? "Grupo de Usuário atualizado com sucesso!" : "Nenhuma atualização realizada.";
                        if (success)
                            await _logClienteRepository.Salvar(new LogCliente() { Id = Guid.NewGuid(), Pagina = "Atualização do Grupo de Usuário", ClienteId = cliente.Id, Acao = JsonConvert.SerializeObject(grupoUsuario), DataRegistro = DateTime.UtcNow }, cancellationToken);
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
