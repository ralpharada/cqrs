namespace Tellus.Domain.Models
{
    public class UsuarioLogado
    {
        public Guid Id { get; set; }
        public Guid UsuarioId { get; set; }
        public Guid ClienteId { get; set; }
        public DateTime UltimaRequisicao { get; set; }
        public string IP { get; set; } = null!;
        public DateTime? Excluido { get; set; } = null!;
    }
}
