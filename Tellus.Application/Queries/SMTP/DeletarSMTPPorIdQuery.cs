using Tellus.Application.Core;
using Tellus.Core.Events;

namespace Tellus.Application.Queries
{
    public class DeletarSMTPPorIdQuery : Request<IEvent>
    {
        public Guid Id { get; private set; }
        public DeletarSMTPPorIdQuery(Guid id)
        {
            Id = id;
        }

    }
}
