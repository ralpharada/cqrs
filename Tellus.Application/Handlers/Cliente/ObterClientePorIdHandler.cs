
using Tellus.Application.Mapper;
using Tellus.Application.Queries;
using Tellus.Application.Responses;
using Tellus.Core.Events;
using Tellus.Domain.Interfaces;
using MediatR;
using Tellus.Application.Core;

namespace Tellus.Application.Handlers
{
    public class ObterClientePorIdHandler : IRequestHandler<ObterClientePorIdQuery, IEvent>
    {
        private readonly UsuarioAutenticado _usuarioAutenticado;
        private readonly IClienteRepository _repositoryCliente;
        private readonly IUsuarioAdmRepository _repository;
        private readonly IProdutoRepository _repositoryProduto;

        public ObterClientePorIdHandler(IClienteRepository repositoryCliente, UsuarioAutenticado usuarioAutenticado, IUsuarioAdmRepository repository, IProdutoRepository repositoryProduto)
        {
            _repositoryCliente = repositoryCliente;
            _repository = repository;
            _usuarioAutenticado = usuarioAutenticado;
            _repositoryProduto = repositoryProduto;
        }

        public async Task<IEvent> Handle(ObterClientePorIdQuery request, CancellationToken cancellationToken)
        {
            var success = false;
            var response = new ClienteResponse();
            try
            {
                if (_usuarioAutenticado.Email == null)
                    return new ResultEvent(success, "Acesso expirado");

                var usuario = await _repository.ObterPorEmailCadastroAtivo(_usuarioAutenticado.Email, cancellationToken);
                if (usuario != null)
                {
                    var taskResult = await _repositoryCliente.ObterPorId(request.Id, cancellationToken);
                    if (taskResult.ProdutoIds != null)
                        taskResult.Produtos = await _repositoryProduto.ObterPorIds(taskResult.ProdutoIds, cancellationToken);
                    response = ClienteMapper<ClienteResponse>.Map(taskResult);
                    success = true;
                }
            }
            catch (Exception ex)
            {
                return new ResultEvent(success, ex.Message);
            }
            return new ResultEvent(success, response);
        }

    }
}
