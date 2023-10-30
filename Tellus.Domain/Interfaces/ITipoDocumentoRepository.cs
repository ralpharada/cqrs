using Tellus.Domain.Models;
using MongoDB.Driver;

namespace Tellus.Domain.Interfaces
{
    public interface ITipoDocumentoRepository
    {
        Task<IEnumerable<TipoDocumento>> ObterTodosPorClienteId(Guid clienteId, string filtro, int currentPage, int pageSize, CancellationToken cancellationToken);
        Task<TipoDocumento> ObterPorId(Guid id, Guid clienteId, CancellationToken cancellationToken);
        Task<ReplaceOneResult> Salvar(TipoDocumento entity, CancellationToken cancellationToken);
        Task<bool> DeletarPorId(Guid id, Guid clienteId, CancellationToken cancellationToken);
        List<TipoDocumento> ObterTodosPorIds(List<Guid> ids, Guid clienteId);
        Task<long> Count(Guid clienteId, CancellationToken cancellationToken);
        Task<IEnumerable<TipoDocumento>> ObterTodosCompleto(Guid clienteId, CancellationToken cancellationToken);
    }

}
