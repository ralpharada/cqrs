using Tellus.Application.Core;
using Tellus.Core.Events;

namespace Tellus.Application.Queries
{
    public class RestaurarDocumentoPorIdQuery : Request<IEvent>
    {
        public Guid Id { get; private set; }
        public RestaurarDocumentoPorIdQuery(Guid id)
        {
            Id = id;
        }

    }
}
