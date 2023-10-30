using Tellus.Application.Mapper;
using Tellus.Application.Queries;
using Tellus.Application.Responses;
using Tellus.Core.Events;
using Tellus.Domain.Interfaces;
using MediatR;

namespace Tellus.Application.Handlers
{
    public class ObterTodosProdutoHandler : IRequestHandler<ObterTodosProdutoQuery, IEvent>
    {
        private readonly IProdutoRepository _repository;

        public ObterTodosProdutoHandler(IProdutoRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEvent> Handle(ObterTodosProdutoQuery request, CancellationToken cancellationToken)
        {
            var success = false;
            var response = new List<ProdutoResponse>();
            try
            {
                var lista = await _repository.ObterTodos(cancellationToken);
                response = ProdutoMapper<List<ProdutoResponse>>.Map(lista);
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
