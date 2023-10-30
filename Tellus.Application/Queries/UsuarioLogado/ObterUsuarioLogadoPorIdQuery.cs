using Tellus.Application.Core;
using Tellus.Core.Events;

namespace Tellus.Application.Queries
{
    public class ObterUsuarioLogadoPorIdQuery : Request<IEvent>
    {
        public Guid Id { get; private set; }
        public ObterUsuarioLogadoPorIdQuery(Guid id)
        {
            Id = id;
        }

    }
}
