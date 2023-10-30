using MongoDB.Bson.Serialization.Attributes;

namespace Tellus.Domain.Models
{
    public partial class GrupoUsuario
    {
        public GrupoUsuario()
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
        public List<Guid> UsuarioIds { get; set; } = null!;
        [BsonIgnoreIfNull]
        public List<Guid> PermissaoIds { get; set; } = null!;
        [BsonIgnoreIfNull]
        public List<TipoDocumento> TipoDocumentos { get; set; } = null!;
        [BsonIgnoreIfNull]
        public List<Usuario> Usuarios { get; set; } = null!;
        [BsonIgnoreIfNull]
        public List<Permissao> Permissoes { get; set; } = null!;
        public List<VinculoPermissao> VinculoPermissoes { get; set; } = null!;
        [BsonIgnoreIfNull]
        public List<Guid> ProdutoIds { get; set; } = null!;
        [BsonIgnoreIfNull]
        public List<Produto> Produtos { get; set; } = null!;
        public bool Bloqueado { get; set; }
    }
}
