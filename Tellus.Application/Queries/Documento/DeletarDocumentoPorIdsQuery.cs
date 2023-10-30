using Tellus.Application.Core;
using Tellus.Core.Events;

namespace Tellus.Application.Queries
{
    public class DeletarDocumentoPorIdsQuery : Request<IEvent>
    {
        public List<Guid> Ids { get; private set; }
        public DeletarDocumentoPorIdsQuery(List<Guid> ids)
        {
            Ids = ids;
        }

    }
}
