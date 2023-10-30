using Tellus.Domain.Models;
using MongoDB.Driver;

namespace Tellus.Domain.Interfaces
{
    public interface IPermissaoRepository
    {
        Task<IEnumerable<Permissao>> ObterTodos(Guid? vinculoId, CancellationToken cancellationToken);
        Task<Permissao> ObterPorId(Guid id, CancellationToken cancellationToken);
        Task<ReplaceOneResult> Salvar(Permissao entity, CancellationToken cancellationToken);
        Task<bool> DeletarPorId(Guid id, CancellationToken cancellationToken);
        List<Permissao> ObterTodosPorIds(List<Guid> ids);
    }

}
