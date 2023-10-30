using Tellus.Application.Core;
using Tellus.Application.Queries;
using Tellus.Core.Events;
using Tellus.Domain.Interfaces;
using MediatR;

namespace Tellus.Application.Handlers
{
    public class AssociarUsuarioProdutoHandler : IRequestHandler<AssociarUsuarioProdutoQuery, IEvent>
    {
        private readonly UsuarioAutenticado _usuarioAutenticado;
        private readonly IClienteRepository _repositoryCliente;
        private readonly IUsuarioRepository _repository;

        public AssociarUsuarioProdutoHandler(IClienteRepository repositoryCliente, UsuarioAutenticado usuarioAutenticado, IUsuarioRepository repository)
        {
            _repositoryCliente = repositoryCliente;
            _repository = repository;
            _usuarioAutenticado = usuarioAutenticado;
        }
        public async Task<IEvent> Handle(AssociarUsuarioProdutoQuery request, CancellationToken cancellationToken)
        {
            string mensagem = "";
            bool success = false;
            try
            {
                if (_usuarioAutenticado.Email == null)
                    return new ResultEvent(success, "Acesso expirado");

                var cliente = await _repositoryCliente.ObterPorEmailCadastroAtivo(_usuarioAutenticado.Email, cancellationToken);
                if (cliente != null)
                {
                    var usuario = await _repository.ObterPorId(request.Id, cancellationToken);
                    if (usuario != null)
                    {
                        usuario.ProdutoIds = request.ProdutoIds;
                        var resultProduto = await _repository.Salvar(usuario, cancellationToken);
                        success = resultProduto.ModifiedCount > 0;
                        mensagem = success ? "Associação com sucesso!" : "Nenhuma atualização realizada.";
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
