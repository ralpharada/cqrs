using Tellus.Core.Events;
using Tellus.Domain.Models;
using MediatR;

namespace Tellus.Application.Queries
{
    public class AdicionarDocumentoQuery : IRequest<IEvent>
    {
        public Guid TipoDocumentoId { get; private set; }
        public List<IndiceValor> IndiceValores { get; private set; } = null!;
        public string NomeArquivo { get; private set; } = null!;
        public long TamanhoArquivo { get; private set; }
        public string FormatoArquivo { get; private set; } = null!;
        public string Arquivo { get; private set; } = null!;
        public AdicionarDocumentoQuery(Guid tipoDocumentoId, List<IndiceValor> indiceValores, string arquivo, string nomeArquivo, long tamanhoArquivo, string formatoArquivo)
        {
            TipoDocumentoId = tipoDocumentoId;
            IndiceValores = indiceValores;
            NomeArquivo = nomeArquivo;
            TamanhoArquivo = tamanhoArquivo;
            FormatoArquivo = formatoArquivo;
            Arquivo = arquivo;
        }
    }
}
