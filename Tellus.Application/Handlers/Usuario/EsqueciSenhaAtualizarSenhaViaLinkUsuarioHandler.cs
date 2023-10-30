using Tellus.Application.Crypto;
using Tellus.Application.Queries;
using Tellus.Application.Util;
using Tellus.Core.Events;
using Tellus.Domain.Interfaces;
using MediatR;

namespace Tellus.Application.Handlers
{
    public class EsqueciSenhaAtualizarSenhaViaLinkUsuarioHandler : IRequestHandler<EsqueciSenhaAtualizarSenhaViaLinkUsuarioQuery, IEvent>
    {
        private readonly IUsuarioRepository _repository;
        private readonly EnviarEmail _enviarEmail;

        public EsqueciSenhaAtualizarSenhaViaLinkUsuarioHandler(IUsuarioRepository repository, EnviarEmail enviarEmail)
        {
            _repository = repository;
            _enviarEmail = enviarEmail;
        }

        public async Task<IEvent> Handle(EsqueciSenhaAtualizarSenhaViaLinkUsuarioQuery request, CancellationToken cancellationToken)
        {
            string mensagem = "";
            var success = false;
            try
            {
                if (String.IsNullOrWhiteSpace(request.Hash))
                {
                    mensagem = "Hash inválido.";
                }
                else
                {
                    if (request.NovaSenha == request.ConfirmarSenha)
                    {
                        var usuario = await _repository.ObterPorHashEsqueciSenha(request.Hash, cancellationToken);
                        if (usuario != null)
                        {
                            usuario.Senha = Criptografia.Encrypt(request.NovaSenha);
                            usuario.HashEsqueciSenha = String.Empty;
                            usuario.DataValidadeEsqueciSenha = null;
                            var result = await _repository.Salvar(usuario, cancellationToken);
                            success = result.ModifiedCount > 0;
                            if (success)
                            {
                                _enviarEmail.Send(usuario.Email, "Atualização de senha", "Seu acesso ao sistema foi atualizado.", null);
                                mensagem = "Seu acesso ao sistema foi atualizado";
                            }
                            else
                            {

                                mensagem = "Falha ao atualizar a senha. Favor tentar novamente";
                            }
                        }
                        else
                        {
                            mensagem = "Hash inválido.";
                        }
                    }
                    else
                    {
                        mensagem = "Confirmação de senha inválida.";
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
