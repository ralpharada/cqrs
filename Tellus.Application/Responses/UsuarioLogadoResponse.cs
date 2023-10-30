namespace Tellus.Application.Responses
{
    public class UsuarioLogadoResponse
    {
        public Guid Id { get; set; }
        public Guid UsuarioId { get; set; }
        public Guid Chave { get; set; }
        public string UsuarioNome { get; set; } = null!;
        public List<Guid> TipoDocumentoIds { get; set; } = null!;
        public List<Guid> Permissoes { get; set; } = null!;
        public List<Guid> Produtos { get; set; } = null!;
        public List<VinculoPermissaoResumidoResponse> VinculoPermissao { get; set; } = null!;
        public List<GrupoUsuarioPermissaoResponse> Grupos { get; set; }
    }
}
