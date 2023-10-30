using Tellus.Application.Core;
using Tellus.Core.Events;

namespace Tellus.Application.Queries
{
    public class DeletarPermissaoPorIdQuery : Request<IEvent>
    {
        public Guid Id { get; private set; }
        public DeletarPermissaoPorIdQuery(Guid id)
        {
            Id = id;
        }

    }
}
