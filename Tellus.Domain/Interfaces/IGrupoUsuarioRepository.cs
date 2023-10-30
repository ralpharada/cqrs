using Tellus.Domain.Models;
using MongoDB.Driver;

namespace Tellus.Domain.Interfaces
{
    public interface IGrupoUsuarioRepository
    {
        Task<GrupoUsuario> ObterPorId(Guid id, Guid clienteId, CancellationToken cancellationToken);
        Task<ReplaceOneResult> Salvar(GrupoUsuario entity, CancellationToken cancellationToken);
        Task<bool> DeletarPorId(Guid id, Guid clienteId, CancellationToken cancellationToken);
        Task<IEnumerable<GrupoUsuario>> ObterTodosPorClienteId(Guid clienteId, string filtro, int currentPage, int pageSize, CancellationToken cancellationToken);
        Task<bool> BloquearPorId(Guid id, Guid clienteId, CancellationToken cancellationToken);
        Task<long> Count(Guid clienteId, CancellationToken cancellationToken);
        Task<IEnumerable<GrupoUsuario>> ObterTodos(Guid clienteId, CancellationToken cancellationToken);
        IEnumerable<GrupoUsuario> ObterTodosPorUsuarioId(Guid usuarioId, Guid clienteId);
    }

}
