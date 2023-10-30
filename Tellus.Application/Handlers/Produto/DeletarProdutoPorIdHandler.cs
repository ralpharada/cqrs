using Tellus.Application.Queries;
using Tellus.Core.Events;
using Tellus.Domain.Interfaces;
using MediatR;

namespace Tellus.Application.Handlers
{
    public class DeletarProdutoPorIdHandler : IRequestHandler<DeletarProdutoPorIdQuery, IEvent>
    {
        private readonly IProdutoRepository _repository;
        public DeletarProdutoPorIdHandler(IProdutoRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEvent> Handle(DeletarProdutoPorIdQuery request, CancellationToken cancellationToken)
        {
            string mensagem = "";
            var success = false;
            try
            {
                success = await _repository.DeletarPorId(request.Id, cancellationToken);
                mensagem = success ? "Produto excluído com sucesso!" : "Não foi possível excluir o Produto.";
            }
            catch (Exception ex)
            {
                mensagem = ex.Message;
            }
            return new ResultEvent(success, mensagem);
        }

    }
}
