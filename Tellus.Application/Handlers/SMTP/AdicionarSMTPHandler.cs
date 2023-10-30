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
    public class AdicionarSMTPHandler : IRequestHandler<AdicionarSMTPQuery, IEvent>
    {
        private readonly ISMTPRepository _repository;
        private readonly IClienteRepository _repositoryCliente;
        private readonly UsuarioAutenticado _usuarioAutenticado;
        private readonly IConfiguration _configuration;
        private readonly ILogClienteRepository _logClienteRepository;

        public AdicionarSMTPHandler(ISMTPRepository repository, UsuarioAutenticado usuarioAutenticado, IClienteRepository repositoryCliente, IConfiguration configuration, ILogClienteRepository logClienteRepository)
        {
            _repository = repository;
            _repositoryCliente = repositoryCliente;
            _usuarioAutenticado = usuarioAutenticado;
            _configuration = configuration;
            _logClienteRepository = logClienteRepository;
        }
        public async Task<IEvent> Handle(AdicionarSMTPQuery request, CancellationToken cancellationToken)
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
                    var smtp = new SMTP
                    {
                        Id = Guid.NewGuid(),
                        Login = request.Login,
                        ClienteId = cliente.Id,
                        Senha = new Criptografia(_configuration).EncryptRsa(request.Senha),
                        Porta = request.Porta,
                        Ssl = request.Ssl,
                        Servidor = request.Servidor,
                        Email = request.Email,
                        Principal = request.Principal,
                        CredencialPadrao = request.DefaultCredential
                    };
                    var result = await _repository.Salvar(smtp, cancellationToken);
                    success = result.UpsertedId > 0;
                    if (success)
                    {
                        id = ((Guid)result.UpsertedId);
                        mensagem = "SMTP cadastrado com sucesso!";
                        await _logClienteRepository.Salvar(new LogCliente() { Id = Guid.NewGuid(), Pagina = "Criação do SMTP", ClienteId = cliente.Id, Acao = JsonConvert.SerializeObject(smtp), DataRegistro = DateTime.UtcNow }, cancellationToken);
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
