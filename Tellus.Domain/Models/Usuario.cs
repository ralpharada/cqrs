using MongoDB.Bson.Serialization.Attributes;

namespace Tellus.Domain.Models
{
    public partial class Usuario
    {
        public Guid Id { get; set; }
        public Guid ClienteId { get; set; }
        [BsonIgnoreIfNull]
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
        [BsonIgnoreIfNull]
        public DateTime DataCadastro { get; set; }
        [BsonIgnoreIfNull]
        public DateTime? DataInativacao { get; set; }
        [BsonIgnoreIfNull]
        public DateTime? DataAtivacaoCadastro { get; set; }
        [BsonIgnoreIfNull]
        public DateTime? DataValidadeAtivacaoCadastro { get; set; }
        public bool Status { get; set; }

        [BsonIgnoreIfNull]
        public List<Guid> TipoDocumentoIds { get; set; } = null!;
        [BsonIgnoreIfNull]
        public List<Guid> ProdutoIds { get; set; } = null!;
        [BsonIgnoreIfNull]
        public List<Produto> Produtos { get; set; } = null!;
        [BsonIgnoreIfNull]
        public List<TipoDocumento> TipoDocumentos { get; set; } = null!;

        [BsonIgnoreIfNull]
        public List<GrupoUsuario> Grupos { get; set; } = null!;

        [BsonIgnoreIfNull]
        public List<Guid> PermissaoIds { get; set; } = null!;

        [BsonIgnoreIfNull]
        public List<Permissao> Permissoes { get; set; } = null!;
        [BsonIgnoreIfNull]
        public List<VinculoPermissao> VinculoPermissoes { get; set; } = null!;
        [BsonIgnoreIfNull]
        public bool? Excluido { get; set; }
        [BsonIgnoreIfNull]
        public DateTime? DataExclusao { get; set; }
    }
}
