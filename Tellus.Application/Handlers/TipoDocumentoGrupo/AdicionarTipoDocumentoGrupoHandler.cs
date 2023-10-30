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
    public class AdicionarTipoDocumentoGrupoHandler : IRequestHandler<AdicionarTipoDocumentoGrupoQuery, IEvent>
    {
        private readonly ITipoDocumentoGrupoRepository _repository;
        private readonly IClienteRepository _repositoryCliente;
        private readonly UsuarioAutenticado _usuarioAutenticado;
        private readonly ILogClienteRepository _logClienteRepository;

        public AdicionarTipoDocumentoGrupoHandler(ITipoDocumentoGrupoRepository repository, UsuarioAutenticado usuarioAutenticado, IClienteRepository repositoryCliente, ILogClienteRepository logClienteRepository)
        {
            _repository = repository;
            _repositoryCliente = repositoryCliente;
            _usuarioAutenticado = usuarioAutenticado;
            _logClienteRepository = logClienteRepository;
        }
        public async Task<IEvent> Handle(AdicionarTipoDocumentoGrupoQuery request, CancellationToken cancellationToken)
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
                    var grupoUsuario = TipoDocumentoGrupoMapper<TipoDocumentoGrupo>.Map(request);
                    grupoUsuario.Id = Guid.NewGuid();
                    grupoUsuario.ClienteId = cliente.Id;
                    grupoUsuario.DataCadastro = DateTime.UtcNow;
                    grupoUsuario.DataUltimaAlteracao = DateTime.UtcNow;
                    var result = await _repository.Salvar(grupoUsuario, cancellationToken);
                    success = result.UpsertedId > 0;
                    if (success)
                    {
                        id = ((Guid)result.UpsertedId);
                        mensagem = "Grupo de Documento cadastrado com sucesso!";
                        await _logClienteRepository.Salvar(new LogCliente() { Id = Guid.NewGuid(), Pagina = "Criação do Grupo de Documento", ClienteId = cliente.Id, Acao = JsonConvert.SerializeObject(grupoUsuario), DataRegistro = DateTime.UtcNow }, cancellationToken);
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
