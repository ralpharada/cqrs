using Tellus.Application.Core;
using Tellus.Core.Events;

namespace Tellus.Application.Queries
{
    public class ObterTodosUsuarioPorGrupoUsuarioIdQuery : Request<IEvent>
    {
        public Guid GrupoUsuarioId { get; private set; }
        public ObterTodosUsuarioPorGrupoUsuarioIdQuery(Guid grupoUsuarioId)
        {
            GrupoUsuarioId = grupoUsuarioId;
        }
    }
}
