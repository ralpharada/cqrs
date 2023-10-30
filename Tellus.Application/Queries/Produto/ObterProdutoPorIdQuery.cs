using Tellus.Application.Core;
using Tellus.Core.Events;

namespace Tellus.Application.Queries
{
    public class ObterProdutoPorIdQuery : Request<IEvent>
    {
        public Guid Id { get; private set; }
        public ObterProdutoPorIdQuery(Guid id)
        {
            Id = id;
        }

    }
}
