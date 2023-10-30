using Tellus.Core.Events;
using MediatR;

namespace Tellus.Application.Queries
{
    public class AdicionarTipoDocumentoQuery : IRequest<IEvent>
    {
        public string Nome { get; set; } = null!;
        public bool Status { get; set; }
        public AdicionarTipoDocumentoQuery(string nome, bool status)
        {
            Nome = nome;
            Status = status;
        }
    }
}
