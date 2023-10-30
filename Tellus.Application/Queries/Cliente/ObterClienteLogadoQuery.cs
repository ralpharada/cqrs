using Tellus.Core.Events;
using MediatR;

namespace Tellus.Application.Queries
{
    public class ObterClienteLogadoQuery : IRequest<IEvent>
    {
        public ObterClienteLogadoQuery()
        {
        }

        public void Validate()
        { }

    }
}
