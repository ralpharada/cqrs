using Tellus.Application.Core;
using Tellus.Application.Queries;
using Tellus.Core.Events;
using Tellus.Domain.Interfaces;
using MediatR;

namespace Tellus.Application.Handlers
{
    public class AtualizarChaveClienteLogadoHandler : IRequestHandler<AtualizarChaveClienteLogadoQuery, IEvent>
    {
        private readonly IClienteRepository _repository;
        private readonly UsuarioAutenticado _usuarioAutenticado;
        public AtualizarChaveClienteLogadoHandler(IClienteRepository repository, UsuarioAutenticado usuarioAutenticado)
        {
            _repository = repository;
            _usuarioAutenticado = usuarioAutenticado;
        }

        public async Task<IEvent> Handle(AtualizarChaveClienteLogadoQuery request, CancellationToken cancellationToken)
        {
            string mensagem = "";
            var success = false;
            try
            {
                if (_usuarioAutenticado.Email == null)
                    return new ResultEvent(success, "Acesso expirado");
                var cliente = await _repository.ObterPorEmailCadastroAtivo(_usuarioAutenticado.Email, cancellationToken);
                if (cliente != null)
                {
                    cliente.Chave = Guid.Parse(request.Chave);
                    var result = await _repository.Salvar(cliente, cancellationToken);
                    success = result.ModifiedCount > 0;
                    mensagem = success ? "Atualizado com sucesso!" : "Erro ao atualizar a chave de acesso da API!";
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
