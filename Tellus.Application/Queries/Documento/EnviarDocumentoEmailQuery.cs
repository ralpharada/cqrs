using Tellus.Core.Events;
using MediatR;

namespace Tellus.Application.Queries
{
    public class EnviarDocumentoEmailQuery : IRequest<IEvent>
    {
        public List<Guid> Ids { get; private set; } = null!;
        public string Destinatario { get; private set; }
        public string Assunto { get; private set; } = null!;
        public string Mensagem { get; private set; } = null!;
        public EnviarDocumentoEmailQuery(List<Guid> ids, string destinatario, string mensagem, string assunto)
        {
            Ids = ids;
            Destinatario = destinatario;
            Assunto = assunto;
            Mensagem = mensagem;
        }
    }
}
