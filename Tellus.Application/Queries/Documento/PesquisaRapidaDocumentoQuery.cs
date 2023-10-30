using Tellus.Core.Events;
using MediatR;

namespace Tellus.Application.Queries
{
    public class PesquisaRapidaDocumentoQuery : IRequest<IEvent>
    {
        public Guid TipoDocumentoId { get; private set; }
        public string Pesquisa { get; private set; } = null!;
        public int CurrentPage { get; private set; }

        public PesquisaRapidaDocumentoQuery(Guid tipoDocumentoId, string pesquisa, int currentPage)
        {
            TipoDocumentoId = tipoDocumentoId;
            Pesquisa = pesquisa;
            CurrentPage = currentPage;
        }
    }
}
