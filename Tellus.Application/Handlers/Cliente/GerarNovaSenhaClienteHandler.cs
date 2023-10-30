using Tellus.Application.Crypto;
using Tellus.Application.Queries;
using Tellus.Application.Util;
using Tellus.Core.Events;
using Tellus.Domain.Interfaces;
using MediatR;

namespace Tellus.Application.Handlers
{
    public class GerarNovaSenhaClienteHandler : IRequestHandler<GerarNovaSenhaClienteQuery, IEvent>
    {
        private readonly IClienteRepository _repository;
        private readonly EnviarEmail _enviarEmail;

        public GerarNovaSenhaClienteHandler(IClienteRepository repository, EnviarEmail enviarEmail)
        {
            _repository = repository;
            _enviarEmail = enviarEmail;
        }

        public async Task<IEvent> Handle(GerarNovaSenhaClienteQuery request, CancellationToken cancellationToken)
        {
            string mensagem = "";
            var success = false;
            try
            {
                var cliente = _repository.ObterPorEmail(request.Email, cancellationToken).Result;
                if (cliente != null)
                {
                    var hash = Guid.NewGuid().ToString("N");
                    cliente.HashEsqueciSenha = hash;
                    cliente.DataValidadeEsqueciSenha = DateTime.Now.AddHours(24);
                    var result = await _repository.Salvar(cliente, cancellationToken);
                    success = result.ModifiedCount > 0;
                    if (success)
                    {
                        _enviarEmail.Send(cliente.Email, "Esqueci a senha", "Para ter acesso ao sistema, clique no link abaixo:<br/> <a href=\"https://localhost:5001/portal/api/validarEsqueciSenhaUsuarioViaLink/" + hash + "\">https://localhost:5001/portal/api/validarEsqueciSenhaUsuarioViaLink/" + hash + "</a>", null);
                        mensagem = "Você receberá um e-mail de acesso ao sistema.";
                    }
                }
            }
            catch (Exception ex)
            {
                mensagem = "Falha ao gerar a nova senha. Favor tentar novamente.";
            }
            return new ResultEvent(success, mensagem);
        }

    }
}
