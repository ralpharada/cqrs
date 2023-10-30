using Tellus.Application.Core;
using Tellus.Core.Events;

namespace Tellus.Application.Queries
{
    public class RelatorioDocumentosUltimos12MesesQuery : Request<IEvent>
    {
        public string Tipo { get;private set; }
        public RelatorioDocumentosUltimos12MesesQuery(string tipo)
        {
            Tipo = tipo;
        }
    }
}
