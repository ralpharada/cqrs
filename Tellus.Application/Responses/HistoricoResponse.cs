using Tellus.Domain.Models;

namespace Tellus.Application.Responses
{
    public class HistoricoResponse
    {
        public Guid Id { get; set; }
        public Cliente? Cliente { get; set; }
        public Usuario? Usuario { get; set; }
        public string ClienteNome { get; set; } = null!;
        public string UsuarioNome { get; set; } = null!;
        public DateTime? Data { get; set; }
        public string Tipo { get; set; } = null!;
    }
}
