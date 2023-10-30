using Tellus.Application.Core;
using Tellus.Core.Events;

namespace Tellus.Application.Queries
{
    public class AtualizarSenhaUsuarioQuery : Request<IEvent>
    {
        public string SenhaAtual { get; private set; }
        public string NovaSenha { get; private set; }
        public string ConfirmarSenha { get; private set; }
        public AtualizarSenhaUsuarioQuery(string senhaAtual, string novaSenha, string confirmarSenha)
        {
            SenhaAtual = senhaAtual;
            NovaSenha = novaSenha;
            ConfirmarSenha = confirmarSenha;
        }

    }
}
