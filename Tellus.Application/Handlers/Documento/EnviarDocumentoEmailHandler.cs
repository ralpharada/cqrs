using Tellus.Application.Core;
using Tellus.Application.Queries;
using Tellus.Application.Responses;
using Tellus.Core.Events;
using Tellus.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Mail;
using Tellus.Application.Util;
using System.Text;
using Tellus.Application.Crypto;

namespace Tellus.Application.Handlers
{
    public class EnviarDocumentoEmailHandler : IRequestHandler<EnviarDocumentoEmailQuery, IEvent>
    {
        private readonly IDocumentoRepository _repository;
        private readonly ILogDocumentoRepository _repositoryLogDocumento;
        private readonly ISMTPRepository _repositorySmtp;
        private readonly IClienteRepository _repositoryCliente;
        private readonly IUsuarioRepository _repositoryUsuario;
        private readonly UsuarioAutenticado _usuarioAutenticado;
        private readonly IConfiguration _configuration;
        private readonly IAmazonRepository _repositoryAmazon;

        public EnviarDocumentoEmailHandler(IDocumentoRepository repository, ISMTPRepository repositorySmtp, ILogDocumentoRepository repositoryLogDocumento, IUsuarioRepository repositoryUsuario, UsuarioAutenticado usuarioAutenticado, IClienteRepository repositoryCliente, IAmazonRepository repositoryAmazon, IConfiguration configuration)
        {
            _repository = repository;
            _repositoryLogDocumento = repositoryLogDocumento;
            _repositorySmtp = repositorySmtp;
            _repositoryCliente = repositoryCliente;
            _repositoryUsuario = repositoryUsuario;
            _usuarioAutenticado = usuarioAutenticado;
            _configuration = configuration;
            _repositoryAmazon = repositoryAmazon;
        }
        public async Task<IEvent> Handle(EnviarDocumentoEmailQuery request, CancellationToken cancellationToken)
        {
            bool success = false;
            var response = new SMTPResponse();
            try
            {
                if (_usuarioAutenticado.Email == null)
                    return new ResultEvent(success, "Acesso expirado");

                var usuario = await _repositoryUsuario.ObterPorEmailCadastroAtivo(_usuarioAutenticado.Email, cancellationToken);
                if (usuario != null)
                {
                    var cliente = await _repositoryCliente.ObterPorId(usuario.ClienteId, cancellationToken);
                    if (cliente != null && cliente.Status)
                    {
                        var documentos = await _repository.ObterPorIds(request.Ids, usuario.ClienteId, cancellationToken);
                        var smtps = await _repositorySmtp.ObterPrincipal(usuario.ClienteId, cancellationToken);
                        foreach (var smtp in smtps)
                        {
                            var senha = new Criptografia(_configuration).DecryptRsa(smtp.Senha);
                            SmtpClient smtpClient = new()
                            {
                                Host = smtp.Servidor,
                                Port = Convert.ToInt16(smtp.Porta),
                                UseDefaultCredentials = smtp.CredencialPadrao,
                                Credentials = new NetworkCredential(smtp.Login, senha),
                                EnableSsl = smtp.Ssl
                            };
                            EnviarEmail enviarEmail = new EnviarEmail(smtpClient, _configuration);
                            StringBuilder stringBuilder = new StringBuilder();
                            documentos.ForEach(async x =>
                            {
                                var file = await _repositoryAmazon.Download(_configuration.GetSection("AmazonS2:ACCESS_KEY_ID").Value, _configuration.GetSection("AmazonS2:SECRET_ACCESS_KEY").Value, _configuration.GetSection("AmazonS2:Bucket").Value, cliente.Id.ToString(), x.Id.ToString());

                                stringBuilder.AppendJoin("<br/>", String.Concat("<a href=\"", string.Concat(x.FormatoArquivo, ",", file), "\">", x.NomeArquivo, "</a>"));
                            });
                            string mensagem = String.Concat(request.Mensagem, "<br/><br/>Arquivos:<br/>", stringBuilder.ToString());
                            success = enviarEmail.Send(request.Destinatario, request.Assunto, mensagem, null, null, smtp.Email);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return new ResultEvent(success, ex.Message);
            }
            return new ResultEvent(success, response);
        }

    }
}
