using Tellus.Application.Core;
using Tellus.Core.Events;

namespace Tellus.Application.Queries
{
    public class DeletarTipoDocumentoGrupoPorIdQuery : Request<IEvent>
    {
        public Guid Id { get; private set; }
        public DeletarTipoDocumentoGrupoPorIdQuery(Guid id)
        {
            Id = id;
        }

    }
}
