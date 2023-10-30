using Tellus.Application.Core;
using Tellus.Core.Events;

namespace Tellus.Application.Queries
{
    public class ValidaUsuarioAdmRefreshTokenQuery : Request<IEvent>
    {
        public string RefreshToken { get; }

        public ValidaUsuarioAdmRefreshTokenQuery(string refreshToken)
        {
            RefreshToken = refreshToken;
        }
    }
}
