using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Tellus.Domain.Models
{
    public partial class Arquivo
    {
        public Arquivo()
        {
        }
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public Guid Id { get; set; }
        public string Link { get; set; } = null!;
        public string Nome { get; set; } = null!;
        public string NomeArquivo { get; set; } = null!;
        public int Versao { get; set; }
        public DateTime DataCadastro { get; set; }
        public DateTime? DataExclusao { get; set; }
        public DateTime DataUltimaAlteracao { get; set; }
        public bool Status { get; set; }
    }
}
