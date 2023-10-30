using Tellus.Application.Core;
using Tellus.Core.Events;

namespace Tellus.Application.Queries
{
    public class DeletarTipoDocumentoPorIdQuery : Request<IEvent>
    {
        public Guid Id { get; private set; }
        public DeletarTipoDocumentoPorIdQuery(Guid id)
        {
            Id = id;
        }

    }
}
