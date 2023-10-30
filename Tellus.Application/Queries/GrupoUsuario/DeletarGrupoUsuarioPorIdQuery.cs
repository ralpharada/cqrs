using Tellus.Application.Core;
using Tellus.Core.Events;

namespace Tellus.Application.Queries
{
    public class DeletarGrupoUsuarioPorIdQuery : Request<IEvent>
    {
        public Guid Id { get; private set; }
        public DeletarGrupoUsuarioPorIdQuery(Guid id)
        {
            Id = id;
        }

    }
}
