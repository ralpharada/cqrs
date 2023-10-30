using Tellus.Domain.Models;

namespace Tellus.Domain.Interfaces
{
    public interface IClienteRepository : IRepository<Cliente>
    {
        Task<Cliente> ObterPorEmail(string email, CancellationToken cancellationToken);
        Task<Cliente> ObterPorHashEsqueciSenha(string hash, CancellationToken cancellationToken);
        Task<Cliente> ObterPorHashAtivacaoCadastro(string hash, CancellationToken cancellationToken);
        Task<bool> ExisteCadastroPorEmail(string email);
        Task<Cliente> ObterPorEmailCadastroAtivo(string email, CancellationToken cancellationToken);
    }
}
