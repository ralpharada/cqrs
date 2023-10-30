using Tellus.Application.Core;
using Tellus.Core.Events;

namespace Tellus.Application.Queries
{
    public class AdicionarUsuarioLogadoQuery : Request<IEvent>
    {
        public Guid UsuarioId { get; set; }
        public Guid ClienteId { get; set; }
        public string IP { get; set; } = null!;
        public int QtdeUsuarios { get; set; }
        public AdicionarUsuarioLogadoQuery(Guid usuarioId, Guid clienteId, string ip,int qtdeUsuarios)
        {
            UsuarioId = usuarioId;
            ClienteId = clienteId;
            IP = ip;
            QtdeUsuarios = qtdeUsuarios;
        }
    }
}
