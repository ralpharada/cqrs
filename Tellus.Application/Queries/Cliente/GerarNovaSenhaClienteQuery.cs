using Tellus.Application.Core;
using Tellus.Core.Events;

namespace Tellus.Application.Queries
{
    public class GerarNovaSenhaClienteQuery : Request<IEvent>
    {
        public string Email { get; private set; }
        public GerarNovaSenhaClienteQuery(string email)
        {
            Email = email;
        }

    }
}
