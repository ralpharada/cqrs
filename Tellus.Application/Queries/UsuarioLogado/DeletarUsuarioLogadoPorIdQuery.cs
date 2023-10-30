using Tellus.Application.Core;
using Tellus.Core.Events;

namespace Tellus.Application.Queries
{
    public class DeletarUsuarioLogadoPorIdQuery : Request<IEvent>
    {
        public Guid Id { get; private set; }
        public DeletarUsuarioLogadoPorIdQuery(Guid id)
        {
            Id = id;
        }

    }
}
