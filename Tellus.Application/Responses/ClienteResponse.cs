using Tellus.Domain.Models;

namespace Tellus.Application.Responses
{
    public class ClienteResponse
    {
        public Guid Id { get; set; }
        public string Documento { get; set; } = null!;
        public string Nome { get; set; } = null!;
        public string Email { get; set; } = null!;
        public Endereco Endereco { get; set; } = null!;
        public DateTime DataCadastro { get; set; }
        public DateTime? DataAtivacaoCadastro { get; set; }
        public int QtdeUsuarios { get; set; }
        public decimal EspacoDisco { get; set; }
        public bool Status { get; set; }
        public List<ProdutoResponse> Produtos { get; set; }

    }
}
