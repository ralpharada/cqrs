using Tellus.Core.Events;
using MediatR;

namespace Tellus.Application.Queries
{
    public class AssociarUsuarioPermissaoQuery : IRequest<IEvent>
    {
        public List<Guid>PermissaoIds { get; private set; }
        public Guid Id { get; private set; } 
        public AssociarUsuarioPermissaoQuery( List<Guid> permissaoIds, Guid id)
        {
            Id = id;
            PermissaoIds = permissaoIds;
        }
    }
}
