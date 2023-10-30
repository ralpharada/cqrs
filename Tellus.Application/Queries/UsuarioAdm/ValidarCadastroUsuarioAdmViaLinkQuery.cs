using Tellus.Application.Core;
using Tellus.Core.Events;

namespace Tellus.Application.Queries
{
    public class ValidarCadastroUsuarioAdmViaLinkQuery : Request<IEvent>
    {
        public string Parametro { get; private set; }
        public ValidarCadastroUsuarioAdmViaLinkQuery(string parametro)
        {
            Parametro = parametro;
        }
    }
}
