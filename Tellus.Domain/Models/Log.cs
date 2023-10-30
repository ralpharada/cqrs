namespace Tellus.Domain.Models
{
    public partial class Log
    {
        public Log()
        {
        }

        public Guid Id { get; set; }
        public string Mensagem { get; set; } = null!;
        public string Erro { get; set; } = null!;
    }
}
