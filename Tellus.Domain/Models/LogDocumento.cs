namespace Tellus.Domain.Models
{
    public partial class LogDocumento
    {
        public LogDocumento()
        {
        }
        public Guid Id { get; set; }
        public Guid ClienteId { get; set; }
        public Guid DocumentoId { get; set; }
        public DateTime DataRegistro { get; set; }
        public List<LogValor> Valores { get; set; } = null!;
        public string Acao { get; set; } = null!;
        
    }
}
