using Tellus.Domain.Models;
using MongoDB.Driver;

namespace Tellus.Domain.Interfaces
{
    public interface IProdutoRepository
    {
        Task<Produto> ObterPorId(Guid id, CancellationToken cancellationToken);
        Task<ReplaceOneResult> Salvar(Produto entity, CancellationToken cancellationToken);
        Task<bool> DeletarPorId(Guid id, CancellationToken cancellationToken);
        Task<List<Produto>> ObterPorIds(List<Guid> ids, CancellationToken cancellationToken);
        Task<IEnumerable<Produto>> ObterTodos(CancellationToken cancellationToken);
    }

}
