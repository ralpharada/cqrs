using Tellus.Application.Core;
using Tellus.Core.Events;

namespace Tellus.Application.Queries
{
    public class ObterTodosPermissaoQuery : Request<IEvent>
    {
        public Guid? VinculoId { get;private set; }
        public ObterTodosPermissaoQuery(Guid? vinculoId)
        {
            VinculoId = vinculoId;
        }

    }
}
