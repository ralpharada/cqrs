using Tellus.Domain.Models;

namespace Tellus.Application.Responses
{
    public class GrupoUsuarioPermissaoResponse
    {
        public Guid Id { get; set; }
        public List<Guid> PermissaoIds { get; set; } = null!;
        public List<Guid> TipoDocumentoIds { get; set; } = null!;
        public List<VinculoPermissaoResumidoResponse> VinculoPermissoes { get; set; } = null!;
    }
}
