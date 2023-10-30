using MongoDB.Bson.Serialization.Attributes;

namespace Tellus.Domain.Models
{
    public partial class SMTP
    {
        public SMTP()
        {
        }
        public Guid Id { get; set; }
        public Guid ClienteId { get; set; }
        [BsonIgnoreIfNull]
        public string Servidor { get; set; } = null!;
        [BsonIgnoreIfNull]
        public string Porta { get; set; } = null!;
        [BsonIgnoreIfNull]
        public string Login { get; set; } = null!;
        [BsonIgnoreIfNull]
        public string Senha { get; set; } = null!;
        [BsonIgnoreIfNull]
        public string Email { get; set; } = null!;
        [BsonIgnoreIfNull]
        public bool Ssl { get; set; }
        public bool Principal { get; set; }
        public bool CredencialPadrao { get; set; }
    }
}
