namespace Tellus.Application.Responses
{
    public class UsuarioAdmResponse
    {
        public Guid Id { get; set; }
        public string Nome { get; set; } = null!;
        public string Email { get; set; } = null!;
        public DateTime DataCadastro { get; set; }
        public DateTime? DataInativacao { get; set; }
        public DateTime? DataAtivacaoCadastro { get; set; }
        public bool Status { get; set; }
        public bool Bloqueado { get; set; }
    }
}
