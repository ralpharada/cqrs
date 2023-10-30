namespace Tellus.Application.Responses
{
    public class ProdutoResponse
    {
        public Guid Id { get; set; }
        public string Titulo { get; set; } = null!;
        public bool Status { get; set; }
    }
}
