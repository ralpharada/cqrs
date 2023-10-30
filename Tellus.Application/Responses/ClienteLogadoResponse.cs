namespace Tellus.Application.Responses
{
    public class ClienteLogadoResponse
    {
        public string Nome { get; set; } = null!;
        public List<ProdutoResponse> Produtos { get; set; }
        public string Chave { get; set; } = null!;

    }
}
