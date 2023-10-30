namespace Tellus.Domain.Models
{
    public partial class LogCliente
    {
        public LogCliente()
        {
        }
        public Guid Id { get; set; }
        public Guid ClienteId { get; set; }
        public DateTime DataRegistro { get; set; }
        public string Pagina { get; set; } = null!;
        public string Acao { get; set; } = null!;
        
    }
}
