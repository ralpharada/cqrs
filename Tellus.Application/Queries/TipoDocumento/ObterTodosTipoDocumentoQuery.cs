using Tellus.Application.Core;
using Tellus.Core.Events;

namespace Tellus.Application.Queries
{
    public class ObterTodosTipoDocumentoQuery : Request<IEvent>
    {
        public string Filtro { get; private set; }
        public int CurrentPage { get; private set; }
        public ObterTodosTipoDocumentoQuery(string filtro, int currentPage)
        {
            Filtro = filtro.ToLower();
            CurrentPage = currentPage;
        }

    }
}
