using Flunt.Notifications;
using Tellus.Application.Core;
using Tellus.Application.Crypto;
using Tellus.Application.Queries;
using Tellus.Application.Util;
using Tellus.Core.Events;
using Tellus.Domain.Interfaces;
using MediatR;

namespace Tellus.Application.Handlers
{
    public class AtualizarSenhaEsqueciSenhaClienteHandler : IRequestHandler<AtualizarSenhaEsqueciSenhaClienteQuery, IEvent>
    {
        private readonly IClienteRepository _repository;
        private readonly EnviarEmail _enviarEmail;
        public AtualizarSenhaEsqueciSenhaClienteHandler(IClienteRepository repository, EnviarEmail enviarEmail)
        {
            _repository = repository;
            _enviarEmail = enviarEmail;
        }

        public async Task<IEvent> Handle(AtualizarSenhaEsqueciSenhaClienteQuery request, CancellationToken cancellationToken)
        {
            string mensagem = "";
            var success = false;
            try
            {
                var cliente = await _repository.ObterPorHashEsqueciSenha(request.Hash, cancellationToken);
                if (cliente != null)
                {
                    if (request.NovaSenha == request.ConfirmarSenha)
                    {
                        cliente.Senha = Criptografia.Encrypt(request.NovaSenha);
                        cliente.HashEsqueciSenha = String.Empty;
                        cliente.DataValidadeEsqueciSenha = null;
                        var result = await _repository.Salvar(cliente, cancellationToken);
                        success = result.ModifiedCount > 0;

                        _enviarEmail.Send(cliente.Email, "Atualização de senha", "Seu acesso ao sistema foi atualizado.", null);
                        mensagem = "Atualizado com sucesso!";
                    }
                    else
                    {
                        mensagem = "Confirmação de senha não confere com a nova senha inserida, favor digite novamente!";
                    }
                }
                else
                {
                    mensagem = "O link utilizado no e-mail expirou. Será necessário gerar um novo link para redefinir a senha.";
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
