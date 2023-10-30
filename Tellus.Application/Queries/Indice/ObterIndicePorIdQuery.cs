using Tellus.Application.Core;
using Tellus.Core.Events;

namespace Tellus.Application.Queries
{
    public class ObterIndicePorIdQuery : Request<IEvent>
    {
        public Guid Id { get; private set; }
        public ObterIndicePorIdQuery(Guid id)
        {
            Id = id;
        }

    }
}
