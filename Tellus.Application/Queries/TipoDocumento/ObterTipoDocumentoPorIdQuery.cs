using Tellus.Application.Core;
using Tellus.Core.Events;

namespace Tellus.Application.Queries
{
    public class ObterTipoDocumentoPorIdQuery : Request<IEvent>
    {
        public Guid Id { get; private set; }
        public ObterTipoDocumentoPorIdQuery(Guid id)
        {
            Id = id;
        }

    }
}
