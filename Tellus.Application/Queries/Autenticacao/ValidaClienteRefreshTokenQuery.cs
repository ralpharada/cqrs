

using Tellus.Application.Core;
using Tellus.Core.Events;

namespace Tellus.Application.Queries
{
    public class ValidaClienteRefreshTokenQuery : Request<IEvent>
    {
        public string RefreshToken { get; }

        public ValidaClienteRefreshTokenQuery(string refreshToken)
        {
            RefreshToken = refreshToken;
        }
    }
}
