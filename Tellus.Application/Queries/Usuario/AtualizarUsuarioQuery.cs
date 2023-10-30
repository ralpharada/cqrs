using Tellus.Application.Core;
using Tellus.Core.Events;

namespace Tellus.Application.Queries
{
    public class AtualizarUsuarioQuery : Request<IEvent>
    {
        public Guid Id { get; private set; }
        public string Nome { get; private set; }
        public string Email { get; private set; }
        public string Senha { get; private set; }
        public bool Status { get; private set; }
        public AtualizarUsuarioQuery(Guid id,  string nome, string email,string senha, bool status)
        {
            Id = id;
            Nome = nome;
            Email = email;
            Senha = senha;
            Status = status;
        }
    }
}
