using Tellus.Application.Core;
using Tellus.Core.Events;

namespace Tellus.Application.Queries
{
    public class ObterTipoDocumentoGrupoPorClienteIdQuery : Request<IEvent>
    {
        public Guid Id { get; private set; }
        public ObterTipoDocumentoGrupoPorClienteIdQuery(Guid id)
        {
            Id = id;
        }

    }
}
