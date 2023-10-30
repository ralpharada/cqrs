using Tellus.Domain.Models;
using MongoDB.Driver;

namespace Tellus.Domain.Interfaces
{
    public interface ILogDocumentoRepository
    {
        Task<ReplaceOneResult> Salvar(LogDocumento entity, CancellationToken cancellationToken);
        Task<List<LogDocumento>> ObterTodosPorDocumentoId(Guid id, Guid clienteId, CancellationToken cancellationToken);
    }

}
