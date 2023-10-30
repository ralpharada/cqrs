using Tellus.Application.Core;
using Tellus.Core.Events;
using Tellus.Domain.Models;

namespace Tellus.Application.Queries
{
    public class AdicionarClienteQuery : Request<IEvent>
    {
        public string Documento { get; private set; }
        public string Nome { get; private set; } 
        public Endereco Endereco { get; private set; } 
        public int QtdeUsuarios { get; private set; }
        public decimal EspacoDisco { get; private set; }
        public string Email { get; private set; }
        public string Senha { get; private set; }
        public bool Status { get; private set; }
        public AdicionarClienteQuery(string documento, string nome, Endereco endereco, int qtdeUsuarios, decimal espacoDisco, string email, string senha)
        {
            Documento = documento;
            Nome = nome;
            Endereco = endereco;
            QtdeUsuarios = qtdeUsuarios;
            Email = email;
            Senha = senha;
            EspacoDisco = espacoDisco;
            Status = true;
        }
    }
}
