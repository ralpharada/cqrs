using Tellus.Domain.Enums;
using MongoDB.Bson.Serialization.Attributes;

namespace Tellus.Domain.Models
{
    public partial class Indice
    {
        public Indice()
        {
        }
        public Guid Id { get; set; }
        public Guid ClienteId { get; set; }
        public string Nome { get; set; } = null!;
        public ETipoIndice ETipoIndice { get; set; }
        [BsonIgnoreIfNull]
        public string Tamanho { get; set; } = null!;
        [BsonIgnoreIfNull]
        public string Mascara { get; set; } = null!;
        public DateTime DataCadastro { get; set; }
        [BsonIgnoreIfNull]
        public DateTime? DataExclusao { get; set; }
        public DateTime DataUltimaAlteracao { get; set; }
        public bool Obrigatorio { get; set; }
        public List<String> Lista { get; set; } = null!;
        public int Ordem { get; set; }
    }
}
