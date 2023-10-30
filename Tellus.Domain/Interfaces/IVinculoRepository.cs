using Tellus.Domain.Models;
using MongoDB.Driver;

namespace Tellus.Domain.Interfaces
{
    public interface IVinculoRepository
    {
        Task<IEnumerable<Vinculo>> ObterTodos(CancellationToken cancellationToken);
        Task<Vinculo> ObterPorId(Guid id, CancellationToken cancellationToken);
        Task<ReplaceOneResult> Salvar(Vinculo entity, CancellationToken cancellationToken);
        Task<bool> DeletarPorId(Guid id, CancellationToken cancellationToken);
    }

}
