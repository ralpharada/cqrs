using Tellus.Application.Core;
using Tellus.Core.Events;

namespace Tellus.Application.Queries
{
    public class ObterPermissaoPorIdQuery : Request<IEvent>
    {
        public Guid Id { get; private set; }
        public ObterPermissaoPorIdQuery(Guid id)
        {
            Id = id;
        }

    }
}
