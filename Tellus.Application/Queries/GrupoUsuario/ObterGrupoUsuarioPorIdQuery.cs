using Tellus.Application.Core;
using Tellus.Core.Events;

namespace Tellus.Application.Queries
{
    public class ObterGrupoUsuarioPorIdQuery : Request<IEvent>
    {
        public Guid Id { get; private set; }
        public ObterGrupoUsuarioPorIdQuery(Guid id)
        {
            Id = id;
        }

    }
}
