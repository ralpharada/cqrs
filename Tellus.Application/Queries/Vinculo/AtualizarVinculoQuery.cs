using Tellus.Core.Events;
using MediatR;

namespace Tellus.Application.Queries
{
    public class AtualizarVinculoQuery : IRequest<IEvent>
    {
        public Guid Id { get; private set; }
        public string Nome { get; set; } = null!;
        public bool Status { get; set; }
        public AtualizarVinculoQuery(Guid id, string nome, bool status)
        {
            Id = id;
            Nome = nome;
            Status = status;
        }
    }
}
