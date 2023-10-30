using Tellus.Application.Core;
using Tellus.Core.Events;

namespace Tellus.Application.Queries
{
    public class ValidarEsqueciSenhaClienteViaLinkQuery : Request<IEvent>
    {
        public string Parametro { get; private set; }
        public ValidarEsqueciSenhaClienteViaLinkQuery(string parametro)
        {
            Parametro = parametro;
        }
    }
}
