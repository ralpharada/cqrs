using Tellus.Application.Queries;
using Tellus.Core.Events;
using Tellus.Domain.Interfaces;
using Tellus.Domain.Models;
using MediatR;
using Tellus.Application.Core;

namespace Tellus.Application.Handlers
{
    public class BloquearUsuarioAdmHandler : IRequestHandler<BloquearUsuarioAdmQuery, IEvent>
    {
        private readonly IUsuarioAdmRepository _repository;
        private readonly UsuarioAutenticado _usuarioAutenticado;
        public BloquearUsuarioAdmHandler(IUsuarioAdmRepository repository, UsuarioAutenticado usuarioAutenticado)
        {
            _repository = repository;
            _usuarioAutenticado = usuarioAutenticado;
        }

        public async Task<IEvent> Handle(BloquearUsuarioAdmQuery request, CancellationToken cancellationToken)
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
                    var usuarioAdm = await _repository.ObterPorId(request.Id, cancellationToken);
                    if (usuarioAdm != null)
                    {
                        usuarioAdm.Bloqueado = !usuarioAdm.Bloqueado;
                        var result = await _repository.Salvar(usuarioAdm, cancellationToken);
                        success = result.ModifiedCount > 0;
                        mensagem = success ? "Usuário atualizado com sucesso!" : "Nenhuma atualização realizada.";
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
