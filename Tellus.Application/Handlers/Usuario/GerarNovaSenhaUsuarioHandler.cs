using Tellus.Application.Crypto;
using Tellus.Application.Queries;
using Tellus.Application.Util;
using Tellus.Core.Events;
using Tellus.Domain.Interfaces;
using MediatR;

namespace Tellus.Application.Handlers
{
    public class GerarNovaSenhaUsuarioHandler : IRequestHandler<GerarNovaSenhaUsuarioQuery, IEvent>
    {
        private readonly IUsuarioRepository _repository;
        private readonly EnviarEmail _enviarEmail;

        public GerarNovaSenhaUsuarioHandler(IUsuarioRepository repository, EnviarEmail enviarEmail)
        {
            _repository = repository;
            _enviarEmail = enviarEmail;
        }

        public async Task<IEvent> Handle(GerarNovaSenhaUsuarioQuery request, CancellationToken cancellationToken)
        {
            string mensagem = "";
            var success = false;
            try
            {
                var Usuario = _repository.ObterPorEmail(request.Email, cancellationToken).Result;
                if (Usuario != null)
                {
                    var hash = Guid.NewGuid().ToString("N");
                    Usuario.HashEsqueciSenha = hash;
                    Usuario.DataValidadeEsqueciSenha = DateTime.Now.AddHours(24);
                    success = await _repository.AtualizarHashEsqueciSenha(Usuario, cancellationToken);
                    if (success)
                    {
                        _enviarEmail.Send(Usuario.Email, "Esqueci a senha", "Para ter acesso ao sistema, clique no link abaixo:<br/> <a href=\"https://localhost:5001/portal/api/validarEsqueciSenhaUsuarioViaLink/" + hash + "\">https://localhost:5001/portal/api/validarEsqueciSenhaUsuarioViaLink/" + hash + "</a>", null);
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
