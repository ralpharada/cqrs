using Tellus.Application.Core;
using Tellus.Core.Events;

namespace Tellus.Application.Queries
{
    public class ObterVinculoPorIdQuery : Request<IEvent>
    {
        public Guid Id { get; private set; }
        public ObterVinculoPorIdQuery(Guid id)
        {
            Id = id;
        }

    }
}
