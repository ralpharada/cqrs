namespace Tellus.Application.Responses
{
    public class SMTPResponse
    {
        public Guid Id { get; set; }
        public string Servidor { get; set; } = null!;
        public string Porta { get; set; } = null!;
        public string Login { get; set; } = null!;
        public string Email { get; set; } = null!;
        public bool Ssl { get; set; }
        public bool Principal { get; set; }
        public bool CredencialPadrao { get; set; }
    }
}
