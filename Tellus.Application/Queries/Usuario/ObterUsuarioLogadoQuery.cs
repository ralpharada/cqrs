using Tellus.Core.Events;
using MediatR;

namespace Tellus.Application.Queries
{
    public class ObterUsuarioLogadoQuery : IRequest<IEvent>
    {
        public ObterUsuarioLogadoQuery()
        {
        }

        public void Validate()
        { }

    }
}
