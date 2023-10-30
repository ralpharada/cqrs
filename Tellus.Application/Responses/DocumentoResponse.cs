namespace Tellus.Application.Responses
{
    public class DocumentoResponse
    {
        public Guid Id { get; set; }
        public TipoDocumentoResponse TipoDocumento { get; set; } = null!;
        public List<IndiceValorResponse> IndiceValores { get; set; } = null!;
        public DateTime DataCadastro { get; set; }
        public DateTime DataUltimaAlteracao { get; set; }
        public string NomeArquivo { get; set; } = null!;
        public long TamanhoArquivo { get; set; }
        public string Arquivo { get; set; } = null!;
        public string FormatoArquivo { get; set; } = null!;
        public DateTime DataExclusao { get; set; }
    }
}
