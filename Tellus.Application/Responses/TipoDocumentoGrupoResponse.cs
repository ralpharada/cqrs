namespace Tellus.Application.Responses
{
    public class TipoDocumentoGrupoResponse
    {
        public Guid Id { get; set; }
        public string Nome { get; set; } = null!;
        public DateTime DataCadastro { get; set; }
        public DateTime DataUltimaAlteracao { get; set; }
        public List<TipoDocumentoResponse> TipoDocumentos { get; set; } = null!;
        public List<Guid> TipoDocumentoIds { get; set; } = null!;
        public bool Status { get; set; }
    }
}
