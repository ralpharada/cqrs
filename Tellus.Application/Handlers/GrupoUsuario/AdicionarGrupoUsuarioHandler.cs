using Tellus.Application.Core;
using Tellus.Application.Mapper;
using Tellus.Application.Queries;
using Tellus.Core.Events;
using Tellus.Domain.Interfaces;
using Tellus.Domain.Models;
using MediatR;
using Newtonsoft.Json;

namespace Tellus.Application.Handlers
{
    public class AdicionarGrupoUsuarioHandler : IRequestHandler<AdicionarGrupoUsuarioQuery, IEvent>
    {
        private readonly IGrupoUsuarioRepository _repository;
        private readonly IClienteRepository _repositoryCliente;
        private readonly UsuarioAutenticado _usuarioAutenticado;
        private readonly ILogClienteRepository _logClienteRepository;

        public AdicionarGrupoUsuarioHandler(IGrupoUsuarioRepository repository, UsuarioAutenticado usuarioAutenticado, IClienteRepository repositoryCliente, ILogClienteRepository logClienteRepository)
        {
            _repository = repository;
            _repositoryCliente = repositoryCliente;
            _usuarioAutenticado = usuarioAutenticado;
            _logClienteRepository = logClienteRepository;
        }
        public async Task<IEvent> Handle(AdicionarGrupoUsuarioQuery request, CancellationToken cancellationToken)
        {
            string mensagem = "";
            bool success = false;
            Guid id = Guid.Empty;
            try
            {
                if (_usuarioAutenticado.Email == null)
                    return new ResultEvent(success, "Acesso expirado");

                var cliente = await _repositoryCliente.ObterPorEmailCadastroAtivo(_usuarioAutenticado.Email, cancellationToken);
                if (cliente != null)
                {
                    var grupoUsuario = GrupoUsuarioMapper<GrupoUsuario>.Map(request);
                    grupoUsuario.Id = Guid.NewGuid();
                    grupoUsuario.ClienteId = cliente.Id;
                    grupoUsuario.DataCadastro = DateTime.UtcNow;
                    grupoUsuario.DataUltimaAlteracao = DateTime.UtcNow;
                    grupoUsuario.Status = true;
                    var result = await _repository.Salvar(grupoUsuario, cancellationToken);
                    success = result.UpsertedId > 0;
                    if (success)
                    {
                        await _logClienteRepository.Salvar(new LogCliente() { Id = Guid.NewGuid(), Pagina = "Criação do Grupo de Usuário", ClienteId = cliente.Id, Acao = JsonConvert.SerializeObject(grupoUsuario), DataRegistro = DateTime.UtcNow }, cancellationToken);
                        id = ((Guid)result.UpsertedId);
                        mensagem = "Grupo de Usuário cadastrado com sucesso!";
                    }
                }
            }
            catch (Exception ex)
            {
                mensagem = ex.Message;
            }
            return new ResultEvent(success, mensagem, null, id);
        }

    }
}
