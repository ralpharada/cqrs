using MongoDB.Bson.Serialization.Attributes;

namespace Tellus.Domain.Models
{
    public partial class Endereco
    {
        public Endereco()
        {
        }
        public string Cep { get; set; } = null!;
        [BsonIgnoreIfNull]
        public string Logradouro { get; set; } = null!;
        [BsonIgnoreIfNull]
        public string Numero { get; set; } = null!;
        [BsonIgnoreIfNull]
        public string Complemento { get; set; } = null!;
        [BsonIgnoreIfNull]
        public string Bairro { get; set; } = null!;
        [BsonIgnoreIfNull]
        public string Cidade { get; set; } = null!;
        [BsonIgnoreIfNull]
        public string Estado { get; set; } = null!;

    }
}
