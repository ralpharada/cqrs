using Tellus.Core.Events;
using MediatR;

namespace Tellus.Application.Queries
{
    public class AtualizarPermissaoQuery : IRequest<IEvent>
    {
        public Guid Id { get; private set; }
        public Guid? VinculoId { get; set; } = null!;
        public string Nome { get; set; } = null!;
        public bool Status { get; set; }
        public AtualizarPermissaoQuery(Guid id, Guid? vinculoId, string nome, bool status)
        {
            Id = id;
            VinculoId = vinculoId;
            Nome = nome;
            Status = status;
        }
    }
}
