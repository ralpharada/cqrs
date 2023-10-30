using MongoDB.Bson.Serialization.Attributes;

namespace Tellus.Domain.Models
{
    public partial class Permissao
    {
        public Permissao()
        {
        }
        public Guid Id { get; set; }
        [BsonIgnoreIfNull]
        public Guid? VinculoId { get; set; }
        public string Nome { get; set; } = null!;
        public bool Status { get; set; }
    }
}
