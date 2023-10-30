using MongoDB.Bson.Serialization.Attributes;

namespace Tellus.Domain.Models
{
    public partial class UsuarioAdm
    {
        public Guid Id { get; set; }
        public string Nome { get; set; } = null!;
        public string Email { get; set; } = null!;
        [BsonIgnoreIfNull]
        public string Senha { get; set; } = null!;
        [BsonIgnoreIfNull]
        public string HashEsqueciSenha { get; set; } = null!;
        [BsonIgnoreIfNull]
        public string HashAtivacaoCadastro { get; set; } = null!;
        [BsonIgnoreIfNull]
        public DateTime? DataValidadeEsqueciSenha { get; set; }
        public DateTime DataCadastro { get; set; }
        [BsonIgnoreIfNull]
        public DateTime? DataInativacao { get; set; }
        [BsonIgnoreIfNull]
        public DateTime? DataAtivacaoCadastro { get; set; }
        [BsonIgnoreIfNull]
        public DateTime? DataValidadeAtivacaoCadastro { get; set; }
        public bool Status { get; set; }
        public bool Bloqueado { get; set; }
        [BsonIgnoreIfNull]
        public bool? Excluido { get; set; }
        [BsonIgnoreIfNull]
        public DateTime? DataExclusao { get; set; }
    }
}
