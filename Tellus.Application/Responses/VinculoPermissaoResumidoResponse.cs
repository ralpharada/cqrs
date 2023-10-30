namespace Tellus.Application.Responses
{
    public class VinculoPermissaoResumidoResponse
    {
        public Guid TipoDocumentoId { get; set; }
        public List<Guid> PermissaoIds { get; set; }
    }
}
