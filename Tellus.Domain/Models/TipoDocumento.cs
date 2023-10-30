using MongoDB.Bson.Serialization.Attributes;

namespace Tellus.Domain.Models
{
    public partial class TipoDocumento
    {
        public TipoDocumento()
        {
        }
        public Guid Id { get; set; }
        public Guid ClienteId { get; set; }
        public Guid GrupoUsuarioId { get; set; }
        public string Nome { get; set; } = null!;
        public DateTime DataCadastro { get; set; }
        [BsonIgnoreIfNull]
        public DateTime? DataExclusao { get; set; }
        public DateTime DataUltimaAlteracao { get; set; }
        public bool Status { get; set; }
        [BsonIgnoreIfNull]
        public List<Guid> IndiceIds { get; set; } = null!;
        public List<Posicao> Posicao { get; set; } = null!;
        [BsonIgnoreIfNull]
        public List<Indice> Indices { get; set; } = null!;
    }
}
