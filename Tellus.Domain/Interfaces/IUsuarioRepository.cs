using Tellus.Domain.Models;
using MongoDB.Driver;

namespace Tellus.Domain.Interfaces
{
    public interface IUsuarioRepository
    {
        Task<IEnumerable<Usuario>> ObterTodosPorClienteId(Guid clienteId, string filtro, int currentPage, int pageSize, CancellationToken cancellationToken);
        Task<Usuario> ObterPorId(Guid id, CancellationToken cancellationToken);
        Task<ReplaceOneResult> Salvar(Usuario entity, CancellationToken cancellationToken);
        Task<bool> DeletarPorId(Guid id, Guid clienteId, CancellationToken cancellationToken);
        Task<bool> AtualizarSenha(Usuario entity, CancellationToken cancellationToken);
        Task<Usuario> ObterPorEmail(string email, CancellationToken cancellationToken);
        Task<Usuario> ObterPorHashEsqueciSenha(string hash, CancellationToken cancellationToken);
        Task<Usuario> ObterPorHashAtivacaoCadastro(string hash, CancellationToken cancellationToken);
        Task<bool> AtualizarHashEsqueciSenha(Usuario entity, CancellationToken cancellationToken);
        Task<Usuario> ObterPorEmailCadastroAtivo(string email, CancellationToken cancellationToken);
        Task<bool> ExisteUsuario(string email, CancellationToken cancellationToken);
        List<Usuario> ObterTodosPorIds(List<Guid> ids, Guid clienteId);
        Task<long> Count(Guid clienteId, string filtro, CancellationToken cancellationToken);
        Task<IEnumerable<Usuario>> ObterTodosCompleto(Guid clienteId, CancellationToken cancellationToken);
    }

}
