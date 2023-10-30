using Tellus.Core.Events;
using MediatR;

namespace Tellus.Application.Queries
{
    public class AssociarUsuarioGrupoUsuarioQuery : IRequest<IEvent>
    {
        public Guid Id { get; private set; }
        public List<Guid> GrupoUsuarioIds { get; private set; }
        public AssociarUsuarioGrupoUsuarioQuery(List<Guid> grupoUsuarioIds, Guid id)
        {
            Id = id;
            GrupoUsuarioIds = grupoUsuarioIds;
        }
    }
}
