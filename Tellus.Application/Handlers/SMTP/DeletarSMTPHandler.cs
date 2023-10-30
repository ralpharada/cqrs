using Tellus.Application.Core;
using Tellus.Application.Queries;
using Tellus.Core.Events;
using Tellus.Domain.Interfaces;
using Tellus.Domain.Models;
using MediatR;
using Newtonsoft.Json;

namespace Tellus.Application.Handlers
{
    public class DeletarSMTPHandler : IRequestHandler<DeletarSMTPPorIdQuery, IEvent>
    {
        private readonly ISMTPRepository _repository;
        private readonly IClienteRepository _repositoryCliente;
        private readonly UsuarioAutenticado _usuarioAutenticado;
        private readonly ILogClienteRepository _logClienteRepository;

        public DeletarSMTPHandler(ISMTPRepository repository, UsuarioAutenticado usuarioAutenticado, IClienteRepository repositoryCliente, ILogClienteRepository logClienteRepository)
        {
            _repository = repository;
            _repositoryCliente = repositoryCliente;
            _usuarioAutenticado = usuarioAutenticado;
            _logClienteRepository = logClienteRepository;
        }
        public async Task<IEvent> Handle(DeletarSMTPPorIdQuery request, CancellationToken cancellationToken)
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
                    var smtp = await _repository.ObterPorId(request.Id, cliente.Id, cancellationToken);
                    success = await _repository.DeletarPorId(request.Id, cliente.Id, cancellationToken);
                    mensagem = success ? "SMTP excluído com sucesso!" : "Não foi possível excluir o SMTP.";
                    if (success)
                        await _logClienteRepository.Salvar(new LogCliente() { Id = Guid.NewGuid(), Pagina = "Exclusão do SMTP", ClienteId = cliente.Id, Acao = JsonConvert.SerializeObject(smtp), DataRegistro = DateTime.UtcNow }, cancellationToken);
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
