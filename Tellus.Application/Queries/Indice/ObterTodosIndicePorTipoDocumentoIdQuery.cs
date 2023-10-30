using Tellus.Application.Core;
using Tellus.Core.Events;

namespace Tellus.Application.Queries
{
    public class ObterTodosIndicePorTipoDocumentoIdQuery : Request<IEvent>
    {
        public Guid TipoDocumentoId { get; private set; }
        public ObterTodosIndicePorTipoDocumentoIdQuery(Guid tipoDocumentoId)
        {
            TipoDocumentoId = tipoDocumentoId;
        }

    }
}
