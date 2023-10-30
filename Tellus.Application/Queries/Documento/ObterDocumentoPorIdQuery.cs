using Tellus.Application.Core;
using Tellus.Core.Events;

namespace Tellus.Application.Queries
{
    public class ObterDocumentoPorIdQuery : Request<IEvent>
    {
        public Guid Id { get; private set; }
        public ObterDocumentoPorIdQuery(Guid id)
        {
            Id = id;
        }

    }
}
