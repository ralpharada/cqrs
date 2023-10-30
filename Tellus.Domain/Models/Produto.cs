namespace Tellus.Domain.Models
{
    public partial class Produto
    {
        public Produto()
        {
        }

        public Guid Id { get; set; }
        public string Titulo { get; set; } = null!;
        public bool Status { get; set; }
    }
}
