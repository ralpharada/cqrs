namespace Tellus.Domain.Models
{
    public partial class Documento
    {
        public Documento()
        {
        }
        public Guid Id { get; set; }
        public Guid ClienteId { get; set; }
        public Guid UsuarioId { get; set; }
        public Guid TipoDocumentoId { get; set; }
        public string JsonIndice { get; set; } = null!;
        public DateTime DataCadastro { get; set; }
        public DateTime DataUltimaAlteracao { get; set; }
        public string NomeArquivo { get; set; } = null!;
        public long TamanhoArquivo { get; set; }
        public string Arquivo { get; set; } = null!;
        public string FormatoArquivo { get; set; } = null!;
        public List<IndiceValor> IndiceValores { get; set; }
        public DateTime? DataExclusao { get; set; }

    }
}
