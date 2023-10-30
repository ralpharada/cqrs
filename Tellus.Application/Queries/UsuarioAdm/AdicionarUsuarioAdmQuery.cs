using Tellus.Application.Core;
using Tellus.Core.Events;

namespace Tellus.Application.Queries
{
    public class AdicionarUsuarioAdmQuery : Request<IEvent>
    {
        public string Nome { get; private set; }
        public string Email { get; private set; }
        public DateTime DataCadastro { get; private set; }
        public string Senha { get; private set; }
        public bool Status { get; private set; }
        public AdicionarUsuarioAdmQuery(string nome, string email, string senha, bool status)
        {
            Nome = nome;
            Email = email;
            Status = status;
            Senha = senha;
            DataCadastro = DateTime.UtcNow;
        }
    }
}
