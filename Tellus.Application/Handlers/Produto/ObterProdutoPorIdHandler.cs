
using Tellus.Application.Mapper;
using Tellus.Application.Queries;
using Tellus.Application.Responses;
using Tellus.Core.Events;
using Tellus.Domain.Interfaces;
using MediatR;

namespace Tellus.Application.Handlers
{
    public class ObterProdutoPorIdHandler : IRequestHandler<ObterProdutoPorIdQuery, IEvent>
    {
        private readonly IProdutoRepository _repository;

        public ObterProdutoPorIdHandler(IProdutoRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEvent> Handle(ObterProdutoPorIdQuery request, CancellationToken cancellationToken)
        {
            var success = false;
            var response = new ProdutoResponse();
            try
            {
                var taskResult = await _repository.ObterPorId(request.Id, cancellationToken);
                response = ProdutoMapper<ProdutoResponse>.Map(taskResult);
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
