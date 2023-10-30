using Tellus.Domain.Models;
using MongoDB.Driver;

namespace Tellus.Domain.Interfaces
{
    public interface ILogClienteRepository
    {
        Task<ReplaceOneResult> Salvar(LogCliente entity, CancellationToken cancellationToken);
        Task<List<LogCliente>> ObterTodosPorClienteId(Guid clienteId, CancellationToken cancellationToken);
    }

}
