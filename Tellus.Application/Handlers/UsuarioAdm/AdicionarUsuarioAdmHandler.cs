using Tellus.Application.Crypto;
using Tellus.Application.Mapper;
using Tellus.Application.Queries;
using Tellus.Application.Util;
using Tellus.Core.Events;
using Tellus.Domain.Interfaces;
using Tellus.Domain.Models;
using MediatR;
using Tellus.Application.Core;

namespace Tellus.Application.Handlers
{
    public class AdicionarUsuarioAdmHandler : IRequestHandler<AdicionarUsuarioAdmQuery, IEvent>
    {
        private readonly IUsuarioAdmRepository _repository;
        private readonly UsuarioAutenticado _usuarioAutenticado;
        private readonly EnviarEmail _enviarEmail;

        public AdicionarUsuarioAdmHandler(IUsuarioAdmRepository repository, UsuarioAutenticado usuarioAutenticado, EnviarEmail enviarEmail)
        {
            _repository = repository;
            _enviarEmail = enviarEmail;
            _usuarioAutenticado = usuarioAutenticado;
        }

        public async Task<IEvent> Handle(AdicionarUsuarioAdmQuery request, CancellationToken cancellationToken)
        {
            string mensagem = "";
            var success = false;
            try
            {
                if (_usuarioAutenticado.Email == null)
                    return new ResultEvent(success, "Acesso expirado");

                var usuario = await _repository.ObterPorEmailCadastroAtivo(_usuarioAutenticado.Email, cancellationToken);
                if (usuario != null)
                {
                    var existsUser = await _repository.ExisteUsuario(request.Email, cancellationToken);
                    if (existsUser)
                    {
                        return new ResultEvent(false, "Jà existe um usuário com esse e-mail");

                    }
                    var usuarioAdm = UsuarioAdmMapper<UsuarioAdm>.Map(request);
                    var hash = Guid.NewGuid().ToString("N");
                    usuarioAdm.Id = Guid.NewGuid();
                    usuarioAdm.HashAtivacaoCadastro = hash;
                    usuarioAdm.DataValidadeAtivacaoCadastro = DateTime.Now.AddHours(24);
                    usuarioAdm.HashEsqueciSenha = String.Empty;
                    usuarioAdm.Senha = Criptografia.Encrypt(request.Senha);
                    var result = await _repository.Salvar(usuarioAdm, cancellationToken);
                    success = result.UpsertedId > 0;
                    if (success)
                    {
                        mensagem = "Cadastro efetuado com sucesso!";
                 //       _enviarEmail.Send(usuarioAdm.Email, "Seja bem vindo!", "Para ter acesso ao sistema, clique no link abaixo:<br/> <a href=\"https://localhost:5001/portal/api/validarCadastroUsuarioAdmViaLink/" + hash + "\">https://localhost:5001/portal/api/validarCadastroUsuarioAdmViaLink/" + hash + "</a>", null);
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
