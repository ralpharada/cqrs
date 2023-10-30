namespace Tellus.Application.Responses
{
    public class IndiceResponse
    {
        public Guid Id { get; set; }
        public string Nome { get; set; } = null!;
        public string ETipoIndice { get; set; } 
        public string Tamanho { get; set; } = null!;
        public string Mascara { get; set; } = null!;
        public DateTime DataCadastro { get; set; }
        public DateTime DataUltimaAlteracao { get; set; }
        public bool Obrigatorio { get; set; }
        public List<String> Lista { get; set; } = null!;
        public int Ordem { get; set; }
    }
}
