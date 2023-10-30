using Tellus.Core.Events;
using MediatR;

namespace Tellus.Application.Queries
{
    public class PesquisaRapidaTiposDocumentoQuery : IRequest<IEvent>
    {
        public string Pesquisa { get; private set; } = null!;

        public PesquisaRapidaTiposDocumentoQuery( string pesquisa)
        {
            Pesquisa = pesquisa;
        }
    }
}
