using Tellus.Application.Core;
using Tellus.Core.Events;

namespace Tellus.Application.Queries
{
    public class ObterTodosTipoDocumentoPorGrupoUsuarioIdQuery : Request<IEvent>
    {
        public Guid GrupoUsuarioId { get; private set; }
        public Guid ClienteId { get; private set; }
        public ObterTodosTipoDocumentoPorGrupoUsuarioIdQuery(Guid grupoUsuarioId, Guid clienteId)
        {
            GrupoUsuarioId = grupoUsuarioId;
            ClienteId = clienteId;
        }

    }
}
