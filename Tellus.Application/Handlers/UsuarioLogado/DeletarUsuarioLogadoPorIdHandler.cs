using Tellus.Application.Core;
using Tellus.Application.Queries;
using Tellus.Core.Events;
using Tellus.Domain.Interfaces;
using MediatR;

namespace Tellus.Application.Handlers
{
    public class DeletarUsuarioLogadoPorIdHandler : IRequestHandler<DeletarUsuarioLogadoPorIdQuery, IEvent>
    {
        private readonly IUsuarioLogadoRepository _repository;
        private readonly IClienteRepository _repositoryCliente;
        private readonly UsuarioAutenticado _usuarioAutenticado;

        public DeletarUsuarioLogadoPorIdHandler(IUsuarioLogadoRepository repository, UsuarioAutenticado usuarioAutenticado, IClienteRepository repositoryCliente)
        {
            _repository = repository;
            _repositoryCliente = repositoryCliente;
            _usuarioAutenticado = usuarioAutenticado;
        }

        public async Task<IEvent> Handle(DeletarUsuarioLogadoPorIdQuery request, CancellationToken cancellationToken)
        {
            string mensagem = "";
            var success = false;
            try
            {
                if (_usuarioAutenticado.Email == null)
                    return new ResultEvent(success, "Acesso expirado");

                var cliente = await _repositoryCliente.ObterPorEmailCadastroAtivo(_usuarioAutenticado.Email, cancellationToken);
                if (cliente != null)
                {
                    success = await _repository.ExcluirPorId(request.Id, cancellationToken);
                    mensagem = success ? "Usuário deslogado com sucesso!" : "Falha ao tentar deslogado o usuário.";
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
