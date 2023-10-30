using Tellus.Core.Events;
using Tellus.Domain.Models;
using MediatR;

namespace Tellus.Application.Queries
{
    public class PesquisarDocumentoQuery : IRequest<IEvent>
    {
        public Guid TipoDocumentoId { get; private set; }
        public List<IndiceValor> IndiceValores { get; private set; } = null!;
        public string? DataImportacaoDe { get; private set; } = null!;
        public string? DataImportacaoAte { get; private set; } = null!;
        public int CurrentPage { get; private set; }

        public PesquisarDocumentoQuery(Guid tipoDocumentoId, List<IndiceValor> indiceValores, string dataImportacaoDe, string dataImportacaoAte, int currentPage)
        {
            TipoDocumentoId = tipoDocumentoId;
            IndiceValores = indiceValores;
            DataImportacaoDe = dataImportacaoDe;
            DataImportacaoAte = dataImportacaoAte;
            CurrentPage = currentPage;
        }
    }
}
