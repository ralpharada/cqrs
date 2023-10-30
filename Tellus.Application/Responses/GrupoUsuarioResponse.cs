using Tellus.Domain.Models;

namespace Tellus.Application.Responses
{
    public class GrupoUsuarioResponse
    {
        public Guid Id { get; set; }
        public string Nome { get; set; } = null!;
        public DateTime DataCadastro { get; set; }
        public DateTime DataUltimaAlteracao { get; set; }
        public List<TipoDocumentoResponse> TipoDocumentos { get; set; } = null!;
        public List<UsuarioResponse> Usuarios { get; set; } = null!;
        public List<Permissao> Permissoes { get; set; } = null!;
        public List<VinculoPermissaoResponse> VinculoPermissao { get; set; } = null!;
        public List<ProdutoResponse> Produtos { get; set; } = null!;
        public bool Status { get; set; }
        public bool Bloqueado { get; set; }
    }
}
