namespace Tellus.Application.Responses
{
    public class ContabilizaConsultaResponse
    {
        public Guid Id { get; set; }
        public string Consulta { get; set; } = null!;
        public string Parametros { get; set; } = null!;
        public string Resultado { get; set; } = null!;
        public DateTime Data { get; set; }
    }
}
