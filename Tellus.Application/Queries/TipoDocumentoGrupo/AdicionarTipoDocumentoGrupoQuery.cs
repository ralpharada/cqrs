using Tellus.Core.Events;
using MediatR;

namespace Tellus.Application.Queries
{
    public class AdicionarTipoDocumentoGrupoQuery : IRequest<IEvent>
    {
        public Guid ClienteId { get; private set; }
        public string Nome { get; private set; } = null!;
        public bool Status { get; private set; }
        public AdicionarTipoDocumentoGrupoQuery(Guid clienteId, string nome, bool status)
        {
            ClienteId = clienteId;
            Nome = nome;
            Status = status;
        }
    }
}
