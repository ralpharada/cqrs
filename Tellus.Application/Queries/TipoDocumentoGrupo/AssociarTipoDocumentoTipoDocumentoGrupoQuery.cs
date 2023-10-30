using Tellus.Core.Events;
using MediatR;

namespace Tellus.Application.Queries
{
    public class AssociarTipoDocumentoTipoDocumentoGrupoQuery : IRequest<IEvent>
    {
        public List<Guid> TipoDocumentoIds { get; private set; }
        public Guid Id { get; private set; }
        public AssociarTipoDocumentoTipoDocumentoGrupoQuery(List<Guid> tipoDocumentoIds, Guid id)
        {
            TipoDocumentoIds = tipoDocumentoIds;
            Id = id;
        }
    }
}
