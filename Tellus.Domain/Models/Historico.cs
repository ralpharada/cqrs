namespace Tellus.Domain.Models
{
    public partial class Historico
    {
        public Historico()
        {
        }
        public Guid Id { get; set; }
        public Guid? ClienteId { get; set; }
        public Guid? UsuarioId { get; set; }
        public string ClienteNome { get; set; } = null!;
        public string UsuarioNome { get; set; } = null!;
        public DateTime? Data { get; set; }
        public string Tipo { get; set; } = null!;
    }
}
