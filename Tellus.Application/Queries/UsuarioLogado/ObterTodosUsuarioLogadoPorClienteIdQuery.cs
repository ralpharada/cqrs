using Tellus.Application.Core;
using Tellus.Core.Events;

namespace Tellus.Application.Queries
{
    public class ObterTodosUsuarioLogadoPorClienteIdQuery : Request<IEvent>
    {
        public int CurrentPage { get; private set; }
        public ObterTodosUsuarioLogadoPorClienteIdQuery(int currentPage)
        {
            CurrentPage = currentPage;
        }
    }
}
