using Tellus.Application.Core;
using Tellus.Core.Events;

namespace Tellus.Application.Queries
{
    public class ObterDocumentosPorIdsQuery : Request<IEvent>
    {
        public List<Guid> Ids { get; private set; }
        public ObterDocumentosPorIdsQuery(List<Guid> ids)
        {
            Ids = ids;
        }

    }
}
