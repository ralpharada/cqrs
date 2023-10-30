using Tellus.Application.Core;
using Tellus.Core.Events;

namespace Tellus.Application.Queries
{
    public class ObterGrupoUsuarioPorUsuarioIdQuery : Request<IEvent>
    {
        public Guid Id { get; private set; }
        public ObterGrupoUsuarioPorUsuarioIdQuery(Guid id)
        {
            Id = id;
        }

    }
}
