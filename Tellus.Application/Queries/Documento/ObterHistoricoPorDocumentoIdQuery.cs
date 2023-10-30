using Tellus.Application.Core;
using Tellus.Core.Events;

namespace Tellus.Application.Queries
{
    public class ObterHistoricoPorDocumentoIdQuery : Request<IEvent>
    {
        public Guid Id { get; private set; }
        public ObterHistoricoPorDocumentoIdQuery(Guid id)
        {
            Id = id;
        }

    }
}
