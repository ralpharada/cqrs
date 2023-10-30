namespace Tellus.Domain.Models
{
    public partial class VinculoPermissao
    {
        public VinculoPermissao()
        {
        }
        public Guid TipoDocumentoId { get; set; }
        public List<Guid> PermissaoIds { get; set; }
    }
}
