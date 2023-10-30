using Tellus.Application.Core;
using Tellus.Core.Events;

namespace Tellus.Application.Queries
{
    public class GerarNovaSenhaUsuarioAdmQuery : Request<IEvent>
    {
        public string Email { get; private set; }
        public GerarNovaSenhaUsuarioAdmQuery(string email)
        {
            Email = email;
        }

    }
}
