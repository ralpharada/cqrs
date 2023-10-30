using Tellus.Application.Core;
using Tellus.Core.Events;

namespace Tellus.Application.Queries
{
    public class DeletarUsuarioAdmPorIdQuery : Request<IEvent>
    {
        public Guid Id { get; private set; }
        public DeletarUsuarioAdmPorIdQuery(Guid id)
        {
            Id = id;
        }

    }
}
