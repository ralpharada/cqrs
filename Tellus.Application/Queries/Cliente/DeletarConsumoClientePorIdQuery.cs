using Tellus.Application.Core;
using Tellus.Core.Events;

namespace Tellus.Application.Queries
{
    public class DeletarConsumoClientePorIdQuery : Request<IEvent>
    {
        public Guid Id { get; private set; }
        public DeletarConsumoClientePorIdQuery(Guid id)
        {
            Id = id;
        }

    }
}
