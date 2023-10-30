using Tellus.Application.Core;
using Tellus.Application.Crypto;
using Tellus.Application.Queries;
using Tellus.Core.Events;
using Tellus.Domain.Interfaces;
using Tellus.Domain.Models;
using MediatR;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace Tellus.Application.Handlers
{
    public class AtualizarSMTPHandler : IRequestHandler<AtualizarSMTPQuery, IEvent>
    {
        private readonly ISMTPRepository _repository;
        private readonly IClienteRepository _repositoryCliente;
        private readonly UsuarioAutenticado _usuarioAutenticado;
        private readonly IConfiguration _configuration;
        private readonly ILogClienteRepository _logClienteRepository;

        public AtualizarSMTPHandler(ISMTPRepository repository, UsuarioAutenticado usuarioAutenticado, IClienteRepository repositoryCliente, IConfiguration configuration, ILogClienteRepository logClienteRepository)
        {
            _repository = repository;
            _repositoryCliente = repositoryCliente;
            _usuarioAutenticado = usuarioAutenticado;
            _configuration = configuration;
            _logClienteRepository = logClienteRepository;
        }
        public async Task<IEvent> Handle(AtualizarSMTPQuery request, CancellationToken cancellationToken)
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
                    if (smtp != null)
                    {
                        if (!String.IsNullOrEmpty(request.Senha))
                            smtp.Senha = new Criptografia(_configuration).EncryptRsa(request.Senha);
                        smtp.Login = request.Login;
                        smtp.Porta = request.Porta;
                        smtp.Ssl = request.Ssl;
                        smtp.Servidor = request.Servidor;
                        smtp.Email = request.Email;
                        smtp.Ssl = request.Ssl;
                        smtp.Principal = request.Principal;
                        smtp.CredencialPadrao = request.DefaultCredential;
                        var result = await _repository.Salvar(smtp, cancellationToken);
                        success = result.ModifiedCount > 0;
                        mensagem = success ? "SMTP atualizado com sucesso!" : "Nenhuma atualização realizada.";
                        if (success)
                            await _logClienteRepository.Salvar(new LogCliente() { Id = Guid.NewGuid(), Pagina = "Atualização do SMTP", ClienteId = cliente.Id, Acao = JsonConvert.SerializeObject(smtp), DataRegistro = DateTime.UtcNow }, cancellationToken);
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
