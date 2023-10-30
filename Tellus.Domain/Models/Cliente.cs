using Tellus.Domain.Enums;
using MongoDB.Bson.Serialization.Attributes;

namespace Tellus.Domain.Models
{
    public partial class Cliente
    {
        public Cliente()
        {
        }

        public Guid Id { get; set; }
        public string Documento { get; set; } = null!;
        public string Nome { get; set; } = null!;
        public string Email { get; set; } = null!;
        public Endereco Endereco { get; set; } = null!;
        public string Senha { get; set; } = null!;
        public string HashEsqueciSenha { get; set; } = null!;
        public string HashAtivacaoCadastro { get; set; } = null!;
        [BsonIgnoreIfNull]
        public DateTime? DataValidadeEsqueciSenha { get; set; }
        public DateTime DataCadastro { get; set; }
        [BsonIgnoreIfNull]
        public DateTime? DataAtivacaoCadastro { get; set; }
        [BsonIgnoreIfNull]
        public DateTime? DataValidadeAtivacaoCadastro { get; set; }
        public int QtdeUsuarios { get; set; }
        public double EspacoDisco { get; set; }
        public bool Status { get; set; }
        [BsonIgnoreIfNull]
        public bool? Excluido { get; set; }
        public DateTime? DataExclusao { get; set; }
        [BsonIgnoreIfNull]
        public List<Guid> ProdutoIds { get; set; } = null!;
        [BsonIgnoreIfNull]
        public List<Produto> Produtos { get; set; } = null!;
        public Guid Chave { get; set; }
    }
}
