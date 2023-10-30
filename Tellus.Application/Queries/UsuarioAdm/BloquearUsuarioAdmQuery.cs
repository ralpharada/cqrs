using Tellus.Application.Core;
using Tellus.Core.Events;

namespace Tellus.Application.Queries
{
    public class BloquearUsuarioAdmQuery : Request<IEvent>
    {
        public Guid Id { get; private set; }
        public BloquearUsuarioAdmQuery(Guid id)
        {
            Id = id;
        }

    }
}
