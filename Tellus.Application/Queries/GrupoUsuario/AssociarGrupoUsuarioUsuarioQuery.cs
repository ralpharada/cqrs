using Tellus.Core.Events;
using MediatR;

namespace Tellus.Application.Queries
{
    public class AssociarGrupoUsuarioUsuarioQuery : IRequest<IEvent>
    {
        public List<Guid> UsuarioIds { get; private set; }
        public Guid Id { get; private set; }
        public AssociarGrupoUsuarioUsuarioQuery(List<Guid> usuarioIds, Guid id)
        {
            UsuarioIds = usuarioIds;
            Id = id;
        }
    }
}
