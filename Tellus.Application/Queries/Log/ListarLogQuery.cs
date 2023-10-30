using Tellus.Application.Core;
using Tellus.Core.Events;

namespace Tellus.Application.Queries
{
    public class ListarLogQuery : Request<IEvent>
    {
        public string DataDe { get; private set; } = null!;
        public string DataAte { get; private set; } = null!;
        public ListarLogQuery(string dataDe, string dataAte)
        {
            DataDe = dataDe;
            DataAte = dataAte;
        }

    }
}
