using Tellus.Core.Events;
using Tellus.Domain.Models;
using MediatR;

namespace Tellus.Application.Queries
{
    public class PosicaoIndiceTipoDocumentoQuery : IRequest<IEvent>
    {
        public Guid TipoDocumentoId { get; private set; }
        public List<Guid> IndiceIds { get; private set; }
        public PosicaoIndiceTipoDocumentoQuery(Guid tipoDocumentoId, List<Guid> indiceIds)
        {
            TipoDocumentoId = tipoDocumentoId;
            IndiceIds = indiceIds;
        }
    }
}
