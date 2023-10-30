using Tellus.Application.Core;
using Tellus.Core.Events;

namespace Tellus.Application.Queries
{
    public class ValidarCadastroUsuarioViaLinkQuery : Request<IEvent>
    {
        public string Parametro { get; private set; }
        public ValidarCadastroUsuarioViaLinkQuery(string parametro)
        {
            Parametro = parametro;
        }
    }
}
