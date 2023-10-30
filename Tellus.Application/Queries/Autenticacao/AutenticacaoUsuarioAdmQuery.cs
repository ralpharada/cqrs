using Tellus.Application.Core;
using Tellus.Core.Events;

namespace Tellus.Application.Queries
{
    public class AutenticacaoUsuarioAdmQuery : Request<IEvent>
    {
        public string Email { get; }
        public string Password { get; }
        public AutenticacaoUsuarioAdmQuery(string email, string password)
        {
            Email = email;
            Password = password;
        }
    }
}
