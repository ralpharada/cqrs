using Tellus.Application.Core;
using Tellus.Core.Events;

namespace Tellus.Application.Queries
{
    public class GerarLogClienteQuery : Request<IEvent>
    {
        public string Pagina { get; private set; }
        public string Acao { get; private set; }
        public GerarLogClienteQuery(string pagina, string acao)
        {
            Pagina = pagina;
            Acao = acao;
        }

    }
}
