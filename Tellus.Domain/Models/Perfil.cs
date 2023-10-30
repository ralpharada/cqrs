using MongoDB.Bson.Serialization.Attributes;

namespace Tellus.Domain.Models
{
    public partial class Perfil
    {
        public Perfil()
        {
        }
        public Guid Id { get; set; }
        public string Nome { get; set; } = null!;
        public bool Status { get; set; }
        [BsonIgnoreIfNull]
        public bool? Excluido { get; set; }
    }
}
