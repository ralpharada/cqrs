using Tellus.Application.Core;
using Tellus.Core.Events;

namespace Tellus.Application.Queries
{
    public class ObterUsuarioPorIdQuery : Request<IEvent>
    {
        public Guid Id { get; private set; }
        public ObterUsuarioPorIdQuery(Guid id)
        {
            Id = id;
        }

    }
}
