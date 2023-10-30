using Tellus.Application.Core;
using Tellus.Core.Events;

namespace Tellus.Application.Queries
{
    public class AdicionarUsuarioQuery : Request<IEvent>
    {
        public string Nome { get; private set; }
        public string Email { get; private set; }
        public string Senha { get; private set; }
        public bool Status { get; private set; }
        public AdicionarUsuarioQuery( string nome, string email,bool status, string senha)
        {
            Nome = nome;
            Email = email;
            Status = status;
            Senha = senha;
        }
    }
}
