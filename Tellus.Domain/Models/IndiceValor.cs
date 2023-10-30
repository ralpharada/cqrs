using MongoDB.Bson.Serialization.Attributes;

namespace Tellus.Domain.Models
{
    public partial class IndiceValor
    {
        public IndiceValor()
        {
        }
        public Guid IndiceId { get; set; }
        [BsonIgnoreIfNull]
        public DateTime? Data { get; set; }
        [BsonIgnoreIfNull]
        public DateTime? Hora { get; set; }
        [BsonIgnoreIfNull]
        public int? Numero { get; set; }
        [BsonIgnoreIfNull]
        public decimal? Decimal { get; set; }
        [BsonIgnoreIfNull]
        public string? Texto { get; set; } = null!;
        [BsonIgnoreIfNull]
        public string ETipoIndice { get; set; } = null!;
        public string? Operador { get; set; } = null!;
    }
}
