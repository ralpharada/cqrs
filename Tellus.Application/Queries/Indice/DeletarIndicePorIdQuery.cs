using Tellus.Application.Core;
using Tellus.Core.Events;

namespace Tellus.Application.Queries
{
    public class DeletarIndicePorIdQuery : Request<IEvent>
    {
        public Guid Id { get; private set; }
        public DeletarIndicePorIdQuery(Guid id)
        {
            Id = id;
        }

    }
}
