using Tellus.Core.Events;
using MediatR;

namespace Tellus.Application.Queries
{
    public class AdicionarVinculoQuery : IRequest<IEvent>
    {
        public string Nome { get; set; } = null!;
        public bool Status { get; set; }
        public AdicionarVinculoQuery(string nome, bool status)
        {
            Nome = nome;
            Status = status;
        }
    }
}
