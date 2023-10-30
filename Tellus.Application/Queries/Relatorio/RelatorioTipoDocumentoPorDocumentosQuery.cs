using Tellus.Application.Core;
using Tellus.Core.Events;

namespace Tellus.Application.Queries
{
    public class RelatorioTipoDocumentoPorDocumentosQuery : Request<IEvent>
    {
        public string Tipo { get;private set; }
        public RelatorioTipoDocumentoPorDocumentosQuery(string tipo)
        {
            Tipo = tipo;
        }
    }
}
