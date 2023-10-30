using MongoDB.Bson.Serialization.Attributes;

namespace Tellus.Domain.Models
{
    public partial class Vinculo
    {
        public Vinculo()
        {
        }
        public Guid Id { get; set; }
        public string Nome { get; set; } = null!;
        public bool Status { get; set; }
    }
}
