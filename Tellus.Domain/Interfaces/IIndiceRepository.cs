using Tellus.Domain.Models;
using MongoDB.Driver;

namespace Tellus.Domain.Interfaces
{
    public interface IIndiceRepository
    {
        Task<IEnumerable<Indice>> ObterTodosPorClienteId(Guid clienteId, CancellationToken cancellationToken);
        List<Indice> ObterTodosPorIds(List<Guid> ids, Guid clienteId);
        Task<Indice> ObterPorId(Guid id, Guid clienteId, CancellationToken cancellationToken);
        Task<ReplaceOneResult> Salvar(Indice entity, CancellationToken cancellationToken);
        Task<bool> DeletarPorId(Guid id, Guid clienteId, CancellationToken cancellationToken);
    }

}
