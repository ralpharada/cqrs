using Tellus.Application.Core;
using Tellus.Core.Events;

namespace Tellus.Application.Queries
{
    public class ValidaUsuarioRefreshTokenQuery : Request<IEvent>
    {
        public string RefreshToken { get; }

        public ValidaUsuarioRefreshTokenQuery(string refreshToken)
        {
            RefreshToken = refreshToken;
        }
    }
}
