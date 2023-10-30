using Tellus.Application.Core;
using Tellus.Core.Events;

namespace Tellus.Application.Queries
{
    public class ObterTipoDocumentoGrupoPorIdQuery : Request<IEvent>
    {
        public Guid Id { get; private set; }
        public ObterTipoDocumentoGrupoPorIdQuery(Guid id)
        {
            Id = id;
        }

    }
}
