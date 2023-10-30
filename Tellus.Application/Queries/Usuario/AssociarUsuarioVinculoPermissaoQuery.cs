using Tellus.Core.Events;
using Tellus.Domain.Models;
using MediatR;

namespace Tellus.Application.Queries
{
    public class AssociarUsuarioVinculoPermissaoQuery : IRequest<IEvent>
    {
        public Guid UsuarioId { get; private set; }
        public List<VinculoPermissao> VinculoPermissao { get; private set; } 
        public AssociarUsuarioVinculoPermissaoQuery(Guid usuarioId, List<VinculoPermissao> vinculoPermissao)
        {
            UsuarioId = usuarioId;
            VinculoPermissao = vinculoPermissao;
        }
    }
}
