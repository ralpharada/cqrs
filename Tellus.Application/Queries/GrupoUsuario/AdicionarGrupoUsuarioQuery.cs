using Tellus.Core.Events;
using MediatR;

namespace Tellus.Application.Queries
{
    public class AdicionarGrupoUsuarioQuery : IRequest<IEvent>
    {
        public Guid ClienteId { get; private set; }
        public string Nome { get; private set; } = null!;
        public AdicionarGrupoUsuarioQuery(Guid clienteId, string nome)
        {
            ClienteId = clienteId;
            Nome = nome;
        }
    }
}
