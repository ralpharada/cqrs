using Tellus.Application.Queries;
using Tellus.Core.Events;
using Tellus.Domain.Interfaces;
using MediatR;
using Tellus.Application.Core;

namespace Tellus.Application.Handlers
{
    public class DeletarUsuarioAdmPorIdHandler : IRequestHandler<DeletarUsuarioAdmPorIdQuery, IEvent>
    {
        private readonly IUsuarioAdmRepository _repository;
        private readonly UsuarioAutenticado _usuarioAutenticado;

        public DeletarUsuarioAdmPorIdHandler(IUsuarioAdmRepository repository, UsuarioAutenticado usuarioAutenticado)
        {
            _repository = repository;
            _usuarioAutenticado = usuarioAutenticado;
        }

        public async Task<IEvent> Handle(DeletarUsuarioAdmPorIdQuery request, CancellationToken cancellationToken)
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
                    success = await _repository.DeletarPorId(request.Id, cancellationToken);
                    mensagem = success ? "Usuário excluído com sucesso!" : "Falha ao tentar excluir o usuário.";
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
