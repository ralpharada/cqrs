using Tellus.Application.Core;
using Tellus.Core.Events;

namespace Tellus.Application.Queries
{
    public class AtualizarChaveClienteLogadoQuery : Request<IEvent>
    {
        public string Chave { get; private set; }
        public AtualizarChaveClienteLogadoQuery(string chave)
        {
            Chave = chave;
        }

    }
}
