using Tellus.Domain.Models;
using MongoDB.Driver;

namespace Tellus.Domain.Interfaces
{
    public interface ISMTPRepository
    {
        Task<IEnumerable<SMTP>> ObterTodosPorClienteId(Guid clienteId, string filtro, int currentPage, int pageSize, CancellationToken cancellationToken);
        Task<SMTP> ObterPorId(Guid id, Guid clienteId, CancellationToken cancellationToken);
        Task<ReplaceOneResult> Salvar(SMTP entity, CancellationToken cancellationToken);
        Task<bool> DeletarPorId(Guid id, Guid clienteId, CancellationToken cancellationToken);
        Task<long> Count(Guid clienteId, CancellationToken cancellationToken);
        Task<List<SMTP>> ObterPrincipal(Guid clienteId, CancellationToken cancellationToken);
    }

}
