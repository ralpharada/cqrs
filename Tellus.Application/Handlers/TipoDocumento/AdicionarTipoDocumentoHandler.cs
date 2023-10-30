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
    public class AdicionarTipoDocumentoHandler : IRequestHandler<AdicionarTipoDocumentoQuery, IEvent>
    {
        private readonly ITipoDocumentoRepository _repository;
        private readonly IClienteRepository _repositoryCliente;
        private readonly UsuarioAutenticado _usuarioAutenticado;
        private readonly ILogClienteRepository _logClienteRepository;

        public AdicionarTipoDocumentoHandler(ITipoDocumentoRepository repository, UsuarioAutenticado usuarioAutenticado, IClienteRepository repositoryCliente, ILogClienteRepository logClienteRepository)
        {
            _repository = repository;
            _repositoryCliente = repositoryCliente;
            _usuarioAutenticado = usuarioAutenticado;
            _logClienteRepository = logClienteRepository;
        }
        public async Task<IEvent> Handle(AdicionarTipoDocumentoQuery request, CancellationToken cancellationToken)
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
                    var tipoDocumento = TipoDocumentoMapper<TipoDocumento>.Map(request);
                    tipoDocumento.Id = Guid.NewGuid();
                    tipoDocumento.ClienteId = cliente.Id;
                    tipoDocumento.DataCadastro = DateTime.UtcNow;
                    tipoDocumento.DataUltimaAlteracao = DateTime.UtcNow;
                    var result = await _repository.Salvar(tipoDocumento, cancellationToken);
                    success = result.UpsertedId > 0;
                    if (success)
                    {
                        id = ((Guid)result.UpsertedId);
                        mensagem = "Tipo Documento cadastrado com sucesso!";
                        await _logClienteRepository.Salvar(new LogCliente() { Id = Guid.NewGuid(), Pagina = "Cadastro do Tipo Documento", ClienteId = cliente.Id, Acao = JsonConvert.SerializeObject(tipoDocumento), DataRegistro = DateTime.UtcNow }, cancellationToken);
                    }
                }
            }
            catch (Exception ex)
            {
                mensagem = "Falha ao tentar efetuar o cadastro.";
            }
            return new ResultEvent(success, mensagem, null, id);
        }

    }
}
