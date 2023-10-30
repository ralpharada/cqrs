using Tellus.Domain.Models;

namespace Tellus.Domain.Interfaces
{
    public interface IUsuarioAdmRepository : IRepository<UsuarioAdm>
    {
        Task<bool> AtualizarSenha(UsuarioAdm entity, CancellationToken cancellationToken);
        Task<UsuarioAdm> ObterPorEmail(string email, CancellationToken cancellationToken);
        Task<UsuarioAdm> ObterPorHashEsqueciSenha(string hash, CancellationToken cancellationToken);
        Task<UsuarioAdm> ObterPorHashAtivacaoCadastro(string hash, CancellationToken cancellationToken);
        Task<bool> AtualizarHashEsqueciSenha(UsuarioAdm entity, CancellationToken cancellationToken);
        Task<UsuarioAdm> ObterPorEmailCadastroAtivo(string email, CancellationToken cancellationToken);
        Task<bool> ExisteUsuario(string email, CancellationToken cancellationToken);
    }

}
