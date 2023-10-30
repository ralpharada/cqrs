using Tellus.Domain.Enums;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace Tellus.Domain.Models
{
    public class RefreshToken
    {
        public Guid Id { get; set; }
        [BsonIgnoreIfNull]
        public Guid? UsuarioId { get; set; }
        [BsonIgnoreIfNull]
        public Guid? ClienteId { get; set; }
        [Key]
        public string Token { get; set; } = null!;
        public DateTime ExpirationDate { get; set; }
        public ETipoUsuario ETipoUsuario { get; set; }
    }
}
