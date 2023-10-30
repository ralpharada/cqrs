using Tellus.Domain.Models;

namespace Tellus.Domain.Interfaces
{
    public interface ILogRepository
    {
        Task<IEnumerable<Log>> Listar(string dataDe, string dataAte, CancellationToken cancellationToken);
    }

}
