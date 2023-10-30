using Tellus.Application.Core;
using Tellus.Core.Events;

namespace Tellus.Application.Queries
{
    public class ObterSMTPPorIdQuery : Request<IEvent>
    {
        public Guid Id { get; private set; }
        public ObterSMTPPorIdQuery(Guid id)
        {
            Id = id;
        }

    }
}
