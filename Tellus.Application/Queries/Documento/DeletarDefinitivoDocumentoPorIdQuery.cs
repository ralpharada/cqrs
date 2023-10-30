using Tellus.Application.Core;
using Tellus.Core.Events;

namespace Tellus.Application.Queries
{
    public class DeletarDefinitivoDocumentoPorIdQuery : Request<IEvent>
    {
        public Guid Id { get; private set; }
        public DeletarDefinitivoDocumentoPorIdQuery(Guid id)
        {
            Id = id;
        }

    }
}
