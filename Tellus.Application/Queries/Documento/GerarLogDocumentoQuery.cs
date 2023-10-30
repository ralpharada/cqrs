using Tellus.Application.Core;
using Tellus.Core.Events;

namespace Tellus.Application.Queries
{
    public class GerarLogDocumentoQuery : Request<IEvent>
    {
        public Guid Id { get; private set; }
        public string Acao { get; private set; }
        public GerarLogDocumentoQuery(Guid id, string acao)
        {
            Id = id;
            Acao = acao;
        }

    }
}
