using Tellus.Core.Events;
using MediatR;

namespace Tellus.Application.Queries
{
    public class AssociarUsuarioTipoDocumentoQuery : IRequest<IEvent>
    {
        public List<Guid> TipoDocumentoIds { get; private set; }
        public Guid Id { get; private set; } 
        public AssociarUsuarioTipoDocumentoQuery( List<Guid> tipoDocumentoIds, Guid id)
        {
            Id = id;
            TipoDocumentoIds = tipoDocumentoIds;
        }
    }
}
