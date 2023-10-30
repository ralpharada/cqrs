using Tellus.Application.Core;
using Tellus.Core.Events;

namespace Tellus.Application.Queries
{
    public class ObterUsuarioAdmPorIdQuery : Request<IEvent>
    {
        public Guid Id { get; private set; }
        public ObterUsuarioAdmPorIdQuery(Guid id)
        {
            Id = id;
        }

    }
}
