using Tellus.Core.Events;
using MediatR;

namespace Tellus.Application.Queries
{
    public class AssociarUsuarioProdutoQuery : IRequest<IEvent>
    {
        public List<Guid>ProdutoIds { get; private set; }
        public Guid Id { get; private set; } 
        public AssociarUsuarioProdutoQuery( List<Guid> produtoIds, Guid id)
        {
            Id = id;
            ProdutoIds = produtoIds;
        }
    }
}
