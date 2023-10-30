using Tellus.Core.Events;
using Tellus.Domain.Models;
using MediatR;

namespace Tellus.Application.Queries
{
    public class AtualizarIndiceValorQuery : IRequest<IEvent>
    {
        public Guid Id { get; private set; }
        public List<IndiceValor> IndiceValores { get; private set; } = null!;
        public AtualizarIndiceValorQuery(Guid id, List<IndiceValor> indiceValores)
        {
            Id = id;
            IndiceValores = indiceValores;
        }
    }
}
