using Tellus.Application.Core;
using Tellus.Core.Events;

namespace Tellus.Application.Queries
{
    public class AtualizarUsuarioLogadoQuery : Request<IEvent>
    {
        public Guid UsuarioId { get; set; }
        public string IP { get; set; } = null!;
        public AtualizarUsuarioLogadoQuery(Guid usuarioId, string ip)
        {
            UsuarioId = usuarioId;
            IP = ip;
        }
    }
}
