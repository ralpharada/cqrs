using Tellus.Core.Events;
using MediatR;

namespace Tellus.Application.Queries
{
    public class AssociarGrupoUsuarioPermissaoQuery : IRequest<IEvent>
    {
        public List<Guid> PermissaoIds { get; private set; }
        public Guid Id { get; private set; }
        public AssociarGrupoUsuarioPermissaoQuery(List<Guid> permissaoIds, Guid id)
        {
            Id = id;
            PermissaoIds = permissaoIds;
        }
    }
}
