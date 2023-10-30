namespace Tellus.Application.Responses
{
    public class LogResponse
    {
        public Guid Id { get; set; }
        public string Mensagem { get; set; } = null!;
        public string Erro { get; set; } = null!;
    }
}
