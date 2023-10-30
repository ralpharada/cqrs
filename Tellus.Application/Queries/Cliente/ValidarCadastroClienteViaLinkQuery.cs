using Tellus.Application.Core;
using Tellus.Core.Events;

namespace Tellus.Application.Queries
{
    public class ValidarCadastroClienteViaLinkQuery : Request<IEvent>
    {
        public string Parametro { get; private set; }
        public ValidarCadastroClienteViaLinkQuery(string parametro)
        {
            Parametro = parametro;
        }
    }
}
