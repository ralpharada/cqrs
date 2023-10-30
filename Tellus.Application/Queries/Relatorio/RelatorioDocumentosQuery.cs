using Tellus.Application.Core;
using Tellus.Core.Events;

namespace Tellus.Application.Queries
{
    public class RelatorioDocumentosQuery : Request<IEvent>
    {
        public string Tipo { get; private set; }
        public RelatorioDocumentosQuery(string tipo)
        {
            Tipo = tipo;
        }
    }
}
