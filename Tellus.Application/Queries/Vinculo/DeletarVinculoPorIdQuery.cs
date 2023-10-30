using Tellus.Application.Core;
using Tellus.Core.Events;

namespace Tellus.Application.Queries
{
    public class DeletarVinculoPorIdQuery : Request<IEvent>
    {
        public Guid Id { get; private set; }
        public DeletarVinculoPorIdQuery(Guid id)
        {
            Id = id;
        }

    }
}
