using Tellus.Core.Events;
using MediatR;

namespace Tellus.Application.Queries
{
    public class AtualizarSMTPQuery : IRequest<IEvent>
    {
        public Guid Id { get; private set; }
        public Guid ClienteId { get; private set; }
        public string Servidor { get; private set; } = null!;
        public string Porta { get; private set; } = null!;
        public string Login { get; private set; } = null!;
        public string Senha { get; private set; } = null!;
        public string Email { get; private set; } = null!;
        public bool Ssl { get; private set; }
        public bool Principal { get; private set; }
        public bool DefaultCredential { get; private set; }
        public AtualizarSMTPQuery(Guid id, Guid clienteId, string servidor, string porta, string login, string senha, string email, bool ssl, bool principal, bool defaultCredential)
        {
            Id = id;
            ClienteId = clienteId;
            Servidor = servidor;
            Porta = porta;
            Login = login;
            Senha = senha;
            Email = email;
            Ssl = ssl;
            Principal = principal;
            DefaultCredential = defaultCredential;
        }
    }
}
