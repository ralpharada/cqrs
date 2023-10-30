using Tellus.Application.Core;
using Tellus.Core.Events;

namespace Tellus.Application.Queries
{
    public class BloquearGrupoUsuarioPorIdQuery : Request<IEvent>
    {
        public Guid Id { get; private set; }
        public Guid ClienteId { get; private set; }
        public BloquearGrupoUsuarioPorIdQuery(Guid id, Guid clienteId)
        {
            Id = id;
            ClienteId = clienteId;
        }

    }
}
