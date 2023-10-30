using MongoDB.Driver;
namespace Tellus.Domain.Interfaces
{
    public interface IRepository<T> where T : class
    {
        Task<IEnumerable<T>> ObterTodos(string filtro, int currentPage, int pageSize, CancellationToken cancellationToken);
        Task<T> ObterPorId(Guid id, CancellationToken cancellationToken);
        Task<ReplaceOneResult> Salvar(T entity, CancellationToken cancellationToken);
        Task<bool> DeletarPorId(Guid id, CancellationToken cancellationToken);
        Task<long> Count(CancellationToken cancellationToken);
    }
}
