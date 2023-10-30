using Tellus.Core.Events;
using MediatR;

namespace Tellus.Application.Queries
{
    public class AssociarTipoDocumentoIndiceQuery : IRequest<IEvent>
    {
        public Guid Id { get; private set; }
        public List<Guid> IndiceIds { get; private set; } 
        public AssociarTipoDocumentoIndiceQuery( Guid id, List<Guid> indiceIds)
        {
            Id = id;
            IndiceIds = indiceIds;
        }
    }
}
