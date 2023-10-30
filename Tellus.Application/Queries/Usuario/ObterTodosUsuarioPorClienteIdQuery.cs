using Tellus.Application.Core;
using Tellus.Core.Events;

namespace Tellus.Application.Queries
{
    public class ObterTodosUsuarioPorClienteIdQuery : Request<IEvent>
    {
        public string Filtro { get; private set; }
        public int CurrentPage { get; private set; }
        public ObterTodosUsuarioPorClienteIdQuery(string filtro, int currentPage)
        {
            Filtro = filtro;
            CurrentPage = currentPage;
        }
    }
}
