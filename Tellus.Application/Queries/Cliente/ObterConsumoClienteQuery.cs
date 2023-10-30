using Tellus.Application.Core;
using Tellus.Core.Events;

namespace Tellus.Application.Queries
{
    public class ObterConsumoClienteQuery : Request<IEvent>
    {
        public Guid Id { get; private set; }
        public Guid ProdutoId { get; private set; }
        public string DataDe { get; private set; }
        public string DataAte { get; private set; }
        public ObterConsumoClienteQuery(Guid id, Guid produtoId, string dataDe, string dataAte)
        {
            Id = id;
            ProdutoId = produtoId;
            DataDe = dataDe;
            DataAte = dataAte;
        }

    }
}
