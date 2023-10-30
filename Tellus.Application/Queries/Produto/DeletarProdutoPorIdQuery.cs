using Tellus.Application.Core;
using Tellus.Core.Events;

namespace Tellus.Application.Queries
{
    public class DeletarProdutoPorIdQuery : Request<IEvent>
    {
        public Guid Id { get; private set; }
        public DeletarProdutoPorIdQuery(Guid id)
        {
            Id = id;
        }

    }
}
