using Tellus.Application.Core;
using Tellus.Core.Events;

namespace Tellus.Application.Queries
{
    public class DeletarDocumentoPorIdQuery : Request<IEvent>
    {
        public Guid Id { get; private set; }
        public DeletarDocumentoPorIdQuery(Guid id)
        {
            Id = id;
        }

    }
}
