using Tellus.Core.Events;
using MediatR;

namespace Tellus.Application.Queries
{
    public class AssociarGrupoUsuarioProdutoQuery : IRequest<IEvent>
    {
        public List<Guid> ProdutoIds { get; private set; }
        public Guid Id { get; private set; }
        public AssociarGrupoUsuarioProdutoQuery(List<Guid> produtoIds, Guid id)
        {
            ProdutoIds = produtoIds;
            Id = id;
        }
    }
}
