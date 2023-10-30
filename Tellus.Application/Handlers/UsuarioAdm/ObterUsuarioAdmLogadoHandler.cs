using Flunt.Notifications;
using Tellus.Application.Core;
using Tellus.Application.Crypto;
using Tellus.Application.Mapper;
using Tellus.Application.Queries;
using Tellus.Application.Responses;
using Tellus.Application.Util;
using Tellus.Core.Events;
using Tellus.Domain.Interfaces;
using Tellus.Domain.Models;
using MediatR;

namespace Tellus.Application.Handlers
{
    public class ObterUsuarioAdmLogadoHandler : IRequestHandler<ObterUsuarioAdmLogadoQuery, IEvent>
    {
        private readonly IUsuarioAdmRepository _repository;
        private readonly UsuarioAutenticado _usuarioAdmAutenticado;
        public ObterUsuarioAdmLogadoHandler(IUsuarioAdmRepository repository, UsuarioAutenticado usuarioAdmAutenticado)
        {
            _repository = repository;
            _usuarioAdmAutenticado = usuarioAdmAutenticado;
        }

        public async Task<IEvent> Handle(ObterUsuarioAdmLogadoQuery request, CancellationToken cancellationToken)
        {
            string mensagem = "";
            try
            {
                if (_usuarioAdmAutenticado.Email == null)
                    return new ResultEvent(false, "Acesso expirado");
                var usuarioAdm = await _repository.ObterPorEmailCadastroAtivo(_usuarioAdmAutenticado.Email, cancellationToken);
                if (usuarioAdm != null)
                {
                    var response = UsuarioAdmMapper<UsuarioAdmLogadoResponse>.Map(usuarioAdm);
                    return new ResultEvent(true, response);
                }
                else
                {
                    mensagem = "Usuário não cadastrado / inativado.";
                }
            }
            catch (Exception ex)
            {
                mensagem = ex.Message;
            }
            return new ResultEvent(false, mensagem);
        }

    }
}
