using Tellus.Application.Core;
using Tellus.Core.Events;

namespace Tellus.Application.Queries
{
    public class EsqueciSenhaAtualizarSenhaViaLinkUsuarioQuery : Request<IEvent>
    {
        public string NovaSenha { get; private set; }
        public string ConfirmarSenha { get; private set; }
        public string Hash { get; private set; }
        public EsqueciSenhaAtualizarSenhaViaLinkUsuarioQuery(string novaSenha, string confirmarSenha, string hash)
        {
            NovaSenha = novaSenha;
            ConfirmarSenha = confirmarSenha;
            Hash = hash;
        }
    }
}
