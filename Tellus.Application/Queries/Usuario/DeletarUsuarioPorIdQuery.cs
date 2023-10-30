using Tellus.Application.Core;
using Tellus.Core.Events;

namespace Tellus.Application.Queries
{
    public class DeletarUsuarioPorIdQuery : Request<IEvent>
    {
        public Guid Id { get; private set; }
        public DeletarUsuarioPorIdQuery(Guid id)
        {
            Id = id;
        }

    }
}
