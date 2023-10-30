using Tellus.Application.Core;
using Tellus.Application.Mapper;
using Tellus.Application.Queries;
using Tellus.Application.Responses;
using Tellus.Core.Events;
using Tellus.Domain.Interfaces;
using MediatR;

namespace Tellus.Application.Handlers
{
    public class ObterClienteLogadoHandler : IRequestHandler<ObterClienteLogadoQuery, IEvent>
    {
        private readonly IClienteRepository _repository;
        private readonly UsuarioAutenticado _usuarioAutenticado;
        private readonly IProdutoRepository _repositoryProduto;

        public ObterClienteLogadoHandler(IClienteRepository repository, UsuarioAutenticado usuarioAutenticado, IProdutoRepository repositoryProduto)
        {
            _repository = repository;
            _usuarioAutenticado = usuarioAutenticado;
            _repositoryProduto = repositoryProduto;
        }

        public async Task<IEvent> Handle(ObterClienteLogadoQuery request, CancellationToken cancellationToken)
        {
            var success = false;
            var response = new ClienteLogadoResponse();
            try
            {
                if (_usuarioAutenticado.Email == null)
                    return new ResultEvent(success, "Acesso expirado");

                var taskResult = await _repository.ObterPorEmailCadastroAtivo(_usuarioAutenticado.Email, cancellationToken);
                if (taskResult.ProdutoIds != null)
                    taskResult.Produtos = await _repositoryProduto.ObterPorIds(taskResult.ProdutoIds, cancellationToken);
                response = ClienteMapper<ClienteLogadoResponse>.Map(taskResult);
                success = true;
            }
            catch (Exception ex)
            {
                return new ResultEvent(success, ex.Message);
            }
            return new ResultEvent(success, response);
        }

    }
}
