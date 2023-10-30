using Tellus.Domain.Models;

namespace Tellus.Application.Responses
{
    public class UsuarioResponse
    {
        public Guid Id { get; set; }
        public Perfil Perfil { get; set; } = null!;
        public string Nome { get; set; } = null!;
        public string Email { get; set; } = null!;
        public DateTime DataCadastro { get; set; }
        public DateTime? DataInativacao { get; set; }
        public DateTime? DataAtivacaoCadastro { get; set; }
        public bool Status { get; set; }
        public List<TipoDocumento> TipoDocumentos { get; set; } = null!;
        public List<GrupoUsuario> Grupos { get; set; } = null!;
        public List<Permissao> Permissoes { get; set; } = null!;
        public List<ProdutoResponse> Produtos { get; set; } = null!;
        public List<VinculoPermissaoResponse> VinculoPermissao { get; set; } = null!;
    }
}
