using Tellus.Application.Core;
using Tellus.Core.Events;

namespace Tellus.Application.Queries
{
    public class ValidarEsqueciSenhaUsuarioViaLinkQuery : Request<IEvent>
    {
        public string Parametro { get; private set; }
        public ValidarEsqueciSenhaUsuarioViaLinkQuery(string parametro)
        {
            Parametro = parametro;
        }
    }
}
