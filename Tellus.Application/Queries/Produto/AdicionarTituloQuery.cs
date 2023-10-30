using Tellus.Application.Core;
using Tellus.Core.Events;
using Tellus.Domain.Models;

namespace Tellus.Application.Queries
{
    public class AdicionarProdutoQuery : Request<IEvent>
    {
        public string Titulo { get; private set; }
        public AdicionarProdutoQuery(string titulo)
        {
            Titulo = titulo;
        }
    }
}
