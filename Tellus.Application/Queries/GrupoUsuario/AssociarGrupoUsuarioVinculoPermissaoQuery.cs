using Tellus.Core.Events;
using Tellus.Domain.Models;
using MediatR;

namespace Tellus.Application.Queries
{
    public class AssociarGrupoUsuarioVinculoPermissaoQuery : IRequest<IEvent>
    {
        public Guid GrupoUsuarioId { get; private set; }
        public List<VinculoPermissao> VinculoPermissao { get; private set; } 
        public AssociarGrupoUsuarioVinculoPermissaoQuery(Guid grupoUsuarioId, List<VinculoPermissao> vinculoPermissao)
        {
            GrupoUsuarioId = grupoUsuarioId;
            VinculoPermissao = vinculoPermissao;
        }
    }
}
