using Tellus.Core.Events;
using MediatR;

namespace Tellus.Application.Queries
{
    public class AssociarClienteProdutoQuery : IRequest<IEvent>
    {
        public List<Guid>ProdutoIds { get; private set; }
        public Guid Id { get; private set; } 
        public AssociarClienteProdutoQuery( List<Guid> produtoIds, Guid id)
        {
            Id = id;
            ProdutoIds = produtoIds;
        }
    }
}
