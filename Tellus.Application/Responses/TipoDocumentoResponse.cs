using Newtonsoft.Json;

namespace Tellus.Application.Responses
{
    public class TipoDocumentoResponse
    {
        public Guid Id { get; set; }
        public string Nome { get; set; } = null!;
        public DateTime DataCadastro { get; set; }
        public DateTime DataUltimaAlteracao { get; set; }
        public bool Status { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<IndiceResponse> Indices { get; set; } = null!;
    }
}
