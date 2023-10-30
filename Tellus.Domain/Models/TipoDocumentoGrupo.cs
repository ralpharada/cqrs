using MongoDB.Bson.Serialization.Attributes;

namespace Tellus.Domain.Models
{
    public partial class TipoDocumentoGrupo
    {
        public TipoDocumentoGrupo()
        {
        }
        public Guid Id { get; set; }
        public Guid ClienteId { get; set; }
        public string Nome { get; set; } = null!;
        public bool Status { get; set; }
        public DateTime DataCadastro { get; set; }
        [BsonIgnoreIfNull]
        public DateTime? DataExclusao { get; set; }
        public DateTime DataUltimaAlteracao { get; set; }
        [BsonIgnoreIfNull]
        public List<Guid> TipoDocumentoIds { get; set; } = null!;
        [BsonIgnoreIfNull]
        public List<TipoDocumento> TipoDocumentos { get; set; } = null!;
    }
}
