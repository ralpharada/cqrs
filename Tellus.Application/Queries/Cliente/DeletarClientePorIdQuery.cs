using Tellus.Application.Core;
using Tellus.Core.Events;

namespace Tellus.Application.Queries
{
    public class DeletarClientePorIdQuery : Request<IEvent>
    {
        public Guid Id { get; private set; }
        public DeletarClientePorIdQuery(Guid id)
        {
            Id = id;
        }

    }
}
