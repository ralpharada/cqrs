using Tellus.Application.Core;
using Tellus.Core.Events;
using Tellus.Domain.Models;

namespace Tellus.Application.Queries
{
    public class AtualizarProdutoQuery : Request<IEvent>
    {
        public Guid Id { get; private set; }
        public string Titulo { get; private set; }
        public bool Status { get; private set; }
        public AtualizarProdutoQuery(Guid id, string titulo, bool status)
        {
            Id = id;
            Titulo = titulo;
            Status = status;
        }
    }
}
