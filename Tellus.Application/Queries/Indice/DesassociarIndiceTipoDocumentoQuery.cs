using Tellus.Core.Events;
using MediatR;

namespace Tellus.Application.Queries
{
    public class DesassociarTipoDocumentoIndiceQuery : IRequest<IEvent>
    {
        public Guid Id { get; private set; }
        public Guid IndiceId { get; private set; }
        public DesassociarTipoDocumentoIndiceQuery(Guid id, Guid indiceId)
        {
            Id = id;
            IndiceId = indiceId;
        }
    }
}
