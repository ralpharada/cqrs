using Tellus.Core.Events;
using MediatR;

namespace Tellus.Application.Queries
{
    public class AtualizarGrupoUsuarioQuery : IRequest<IEvent>
    {
        public Guid ClienteId { get; private set; }
        public Guid Id { get; private set; }
        public string Nome { get; private set; } = null!;
        public bool Status { get; private set; }
        public AtualizarGrupoUsuarioQuery(Guid id, Guid clienteId, string nome, bool status)
        {
            Id = id;
            ClienteId = clienteId;
            Nome = nome;
            Status = status;
        }
    }
}
