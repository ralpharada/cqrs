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
    public class AtualizarSenhaUsuarioAdmHandler : IRequestHandler<AtualizarSenhaUsuarioAdmQuery, IEvent>
    {
        private readonly IUsuarioAdmRepository _repository;
        private readonly UsuarioAutenticado _usuarioAdmAutenticado;
        private readonly EnviarEmail _enviarEmail;
        public AtualizarSenhaUsuarioAdmHandler(IUsuarioAdmRepository repository, UsuarioAutenticado usuarioAdmAutenticado, EnviarEmail enviarEmail)
        {
            _repository = repository;
            _usuarioAdmAutenticado = usuarioAdmAutenticado;
            _enviarEmail = enviarEmail;
        }

        public async Task<IEvent> Handle(AtualizarSenhaUsuarioAdmQuery request, CancellationToken cancellationToken)
        {
            string mensagem = "";
            var success = false;
            try
            {
                if (_usuarioAdmAutenticado.Email == null)
                    return new ResultEvent(success, "Acesso expirado");

                var usuarioAdm = await _repository.ObterPorEmailCadastroAtivo(_usuarioAdmAutenticado.Email, cancellationToken);
                if (usuarioAdm != null && Criptografia.Verify(request.SenhaAtual, usuarioAdm.Senha))
                {
                    usuarioAdm.Senha = Criptografia.Encrypt(request.NovaSenha);
                    success = await _repository.AtualizarSenha(usuarioAdm, cancellationToken);
                    if (success)
                    {
                        _enviarEmail.Send(usuarioAdm.Email, "Atualização de senha", "Sua senha de acesso ao sistema foi atualizado.", null);
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
