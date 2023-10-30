using Tellus.Application.Core;
using Tellus.Core.Events;
using Tellus.Domain.Models;

namespace Tellus.Application.Queries
{
    public class AtualizarClienteQuery : Request<IEvent>
    {
        public Guid Id { get; private set; }
        public string Documento { get; private set; }
        public string Nome { get; private set; }
        public Endereco Endereco { get; private set; }
        public int QtdeUsuarios { get; private set; }
        public double EspacoDisco { get; private set; }
        public string Email { get; private set; }
        public string Senha { get; private set; }
        public bool Status { get; private set; }
        public AtualizarClienteQuery(Guid id, string documento, string nome, Endereco endereco,int qtdeUsuarios, double espacoDisco, string email, string senha, bool status)
        {
            Id = id;
            Documento = documento;
            Nome = nome;
            Endereco = endereco;
            QtdeUsuarios = qtdeUsuarios;
            EspacoDisco = espacoDisco;
            Email = email;
            Senha = senha;
            Status = status;
        }
    }
}
