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
    public class AtualizarSenhaUsuarioHandler : IRequestHandler<AtualizarSenhaUsuarioQuery, IEvent>
    {
        private readonly IUsuarioRepository _repository;
        private readonly UsuarioAutenticado _usuarioAutenticado;
        private readonly EnviarEmail _enviarEmail;
        public AtualizarSenhaUsuarioHandler(IUsuarioRepository repository, UsuarioAutenticado usuarioAutenticado, EnviarEmail enviarEmail)
        {
            _repository = repository;
            _usuarioAutenticado = usuarioAutenticado;
            _enviarEmail = enviarEmail;
        }

        public async Task<IEvent> Handle(AtualizarSenhaUsuarioQuery request, CancellationToken cancellationToken)
        {
            string mensagem = "";
            var success = false;
            try
            {
                if (_usuarioAutenticado.Email == null)
                    return new ResultEvent(success, "Acesso expirado");

                var usuario = await _repository.ObterPorEmailCadastroAtivo(_usuarioAutenticado.Email, cancellationToken);
                if (usuario != null && Criptografia.Verify(request.SenhaAtual, usuario.Senha))
                {
                    usuario.Senha = Criptografia.Encrypt(request.NovaSenha);
                    success = await _repository.AtualizarSenha(usuario, cancellationToken);
                    if (success)
                    {
                        _enviarEmail.Send(usuario.Email, "Atualização de senha", "Sua senha de acesso ao sistema foi atualizado.", null);
                        mensagem = "Atualizado com sucesso!";
                    }
                }
                else
                {
                    mensagem = "Não foi possível atualizar a senha. Verifique se digitou a senha corretamente.";
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
