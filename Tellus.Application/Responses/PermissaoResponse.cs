namespace Tellus.Application.Responses
{
    public class PermissaoResponse
    {
        public Guid Id { get; set; }
        public Guid? VinculoId { get; set; }
        public string Nome { get; set; } = null!;
        public bool Status { get; set; }
    }
}
