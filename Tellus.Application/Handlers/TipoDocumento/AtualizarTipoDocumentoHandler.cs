using Tellus.Application.Core;
using Tellus.Application.Queries;
using Tellus.Core.Events;
using Tellus.Domain.Interfaces;
using Tellus.Domain.Models;
using MediatR;
using Newtonsoft.Json;

namespace Tellus.Application.Handlers
{
    public class AtualizarTipoDocumentoHandler : IRequestHandler<AtualizarTipoDocumentoQuery, IEvent>
    {
        private readonly ITipoDocumentoRepository _repository;
        private readonly IClienteRepository _repositoryCliente;
        private readonly UsuarioAutenticado _usuarioAutenticado;
        private readonly ILogClienteRepository _logClienteRepository;

        public AtualizarTipoDocumentoHandler(ITipoDocumentoRepository repository, UsuarioAutenticado usuarioAutenticado, IClienteRepository repositoryCliente, ILogClienteRepository logClienteRepository)
        {
            _repository = repository;
            _repositoryCliente = repositoryCliente;
            _usuarioAutenticado = usuarioAutenticado;
            _logClienteRepository = logClienteRepository;
        }
        public async Task<IEvent> Handle(AtualizarTipoDocumentoQuery request, CancellationToken cancellationToken)
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
                    if (tipoDocumento != null)
                    {
                        tipoDocumento.Nome = request.Nome;
                        tipoDocumento.Status = request.Status;
                        tipoDocumento.DataUltimaAlteracao = DateTime.UtcNow;
                        var result = await _repository.Salvar(tipoDocumento, cancellationToken);
                        success = result.ModifiedCount > 0;
                        if (success)
                        {
                            mensagem = "Tipo Documento atualizado com sucesso!";
                            await _logClienteRepository.Salvar(new LogCliente() { Id = Guid.NewGuid(), Pagina = "Atualização do Tipo Documento", ClienteId = cliente.Id, Acao = JsonConvert.SerializeObject(tipoDocumento), DataRegistro = DateTime.UtcNow }, cancellationToken);
                        }
                        else
                            mensagem = "Nenhuma atualização realizada.";
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
