using Tellus.Application.Core;
using Tellus.Core.Events;

namespace Tellus.Application.Queries
{
    public class AtualizarSenhaClienteLogadoQuery : Request<IEvent>
    {
        public string SenhaAtual { get; private set; }
        public string NovaSenha { get; private set; }
        public string ConfirmarSenha { get; private set; }
        public AtualizarSenhaClienteLogadoQuery(string senhaAtual, string novaSenha, string confirmarSenha)
        {
            SenhaAtual = senhaAtual;
            NovaSenha = novaSenha;
            ConfirmarSenha = confirmarSenha;
        }

    }
}
