using Tellus.Application.Crypto;
using Tellus.Application.Queries;
using Tellus.Application.Util;
using Tellus.Core.Events;
using Tellus.Domain.Interfaces;
using MediatR;

namespace Tellus.Application.Handlers
{
    public class GerarNovaSenhaUsuarioAdmHandler : IRequestHandler<GerarNovaSenhaUsuarioAdmQuery, IEvent>
    {
        private readonly IUsuarioAdmRepository _repository;
        private readonly EnviarEmail _enviarEmail;

        public GerarNovaSenhaUsuarioAdmHandler(IUsuarioAdmRepository repository, EnviarEmail enviarEmail)
        {
            _repository = repository;
            _enviarEmail = enviarEmail;
        }

        public async Task<IEvent> Handle(GerarNovaSenhaUsuarioAdmQuery request, CancellationToken cancellationToken)
        {
            string mensagem = "";
            var success = false;
            try
            {
                var UsuarioAdm = _repository.ObterPorEmail(request.Email, cancellationToken).Result;
                if (UsuarioAdm != null)
                {
                    var hash = Guid.NewGuid().ToString("N");
                    UsuarioAdm.HashEsqueciSenha = hash;
                    UsuarioAdm.DataValidadeEsqueciSenha = DateTime.Now.AddHours(24);
                    success = await _repository.AtualizarHashEsqueciSenha(UsuarioAdm, cancellationToken);
                    if (success)
                    {
                        _enviarEmail.Send(UsuarioAdm.Email, "Esqueci a senha", "Para ter acesso ao sistema, clique no link abaixo:<br/> <a href=\"https://localhost:5001/portal/api/validarEsqueciSenhaUsuarioAdmViaLink/" + hash + "\">https://localhost:5001/portal/api/validarEsqueciSenhaUsuarioAdmViaLink/" + hash + "</a>", null);
                        mensagem = "Você receberá um e-mail de acesso ao sistema.";
                    }
                    else
                    {

                        mensagem = "Falha ao gerar a nova senha. Favor tentar novamente.";
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
