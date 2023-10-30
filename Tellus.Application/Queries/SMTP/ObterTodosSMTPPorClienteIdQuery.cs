using Tellus.Application.Core;
using Tellus.Core.Events;

namespace Tellus.Application.Queries
{
    public class ObterTodosSMTPPorClienteIdQuery : Request<IEvent>
    {
        public string Filtro { get; private set; }
        public int CurrentPage { get; private set; }
        public ObterTodosSMTPPorClienteIdQuery( string filtro, int currentPage)
        {
            Filtro = filtro.ToLower();
            CurrentPage = currentPage;
        }

    }
}
