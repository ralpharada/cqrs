namespace Tellus.Domain.Models
{
    public partial class ContabilizaConsulta
    {
        public ContabilizaConsulta()
        {
        }
        public Guid Id { get; set; }
        public Guid ClienteId { get; set; }
        public Guid ProdutoId { get; set; }
        public string Consulta { get; set; } = null!;
        public string Parametros { get; set; } = null!;
        public DateTime Data { get; set; }
        public string Resultado { get; set; } = null!;
    }
}
