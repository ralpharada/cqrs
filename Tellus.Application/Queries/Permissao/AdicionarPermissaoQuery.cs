using Tellus.Core.Events;
using MediatR;

namespace Tellus.Application.Queries
{
    public class AdicionarPermissaoQuery : IRequest<IEvent>
    {
        public Guid? VinculoId { get; set; } = null!;
        public string Nome { get; set; } = null!;
        public bool Status { get; set; }
        public AdicionarPermissaoQuery(Guid? vinculoId, string nome, bool status)
        {
            VinculoId = vinculoId;
            Nome = nome;
            Status = status;
        }
    }
}
