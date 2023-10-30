using Tellus.Application.Core;
using Tellus.Core.Events;

namespace Tellus.Application.Queries
{
    public class GerarNovaSenhaUsuarioQuery : Request<IEvent>
    {
        public string Email { get; private set; }
        public GerarNovaSenhaUsuarioQuery(string email)
        {
            Email = email;
        }

    }
}
