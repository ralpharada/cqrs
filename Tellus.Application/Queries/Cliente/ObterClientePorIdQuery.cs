using Tellus.Application.Core;
using Tellus.Core.Events;

namespace Tellus.Application.Queries
{
    public class ObterClientePorIdQuery : Request<IEvent>
    {
        public Guid Id { get; private set; }
        public ObterClientePorIdQuery(Guid id)
        {
            Id = id;
        }

    }
}
