using Tellus.Application.Core;
using Tellus.Application.Mapper;
using Tellus.Application.Queries;
using Tellus.Application.Responses;
using Tellus.Core.Events;
using Tellus.Domain.Interfaces;
using Tellus.Domain.Models;
using MediatR;
using Newtonsoft.Json;

namespace Tellus.Application.Handlers
{
    public class DeletarTipoDocumentoPorIdHandler : IRequestHandler<DeletarTipoDocumentoPorIdQuery, IEvent>
    {
        private readonly ITipoDocumentoRepository _repository;
        private readonly IClienteRepository _repositoryCliente;
        private readonly UsuarioAutenticado _usuarioAutenticado;
        private readonly ILogClienteRepository _logClienteRepository;

        public DeletarTipoDocumentoPorIdHandler(ITipoDocumentoRepository repository, UsuarioAutenticado usuarioAutenticado, IClienteRepository repositoryCliente, ILogClienteRepository logClienteRepository)
        {
            _repository = repository;
            _repositoryCliente = repositoryCliente;
            _usuarioAutenticado = usuarioAutenticado;
            _logClienteRepository = logClienteRepository;
        }
        public async Task<IEvent> Handle(DeletarTipoDocumentoPorIdQuery request, CancellationToken cancellationToken)
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
                    var tipoDocumento = await _repository.ObterPorId(request.Id, cliente.Id, cancellationToken);
                    success = await _repository.DeletarPorId(request.Id, cliente.Id, cancellationToken);
                    mensagem = success ? "Tipo Documento excluído com sucesso!" : "Não foi possível excluir o Tipo Documento.";
                    if (success)
                        await _logClienteRepository.Salvar(new LogCliente() { Id = Guid.NewGuid(), Pagina = "Atualização do Tipo Documento", ClienteId = cliente.Id, Acao = JsonConvert.SerializeObject(tipoDocumento), DataRegistro = DateTime.UtcNow }, cancellationToken);
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
