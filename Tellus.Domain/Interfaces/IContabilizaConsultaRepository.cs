using Tellus.Domain.Models;
using MongoDB.Driver;

namespace Tellus.Domain.Interfaces
{
    public interface IContabilizaConsultaRepository
    {
        Task<ReplaceOneResult> Salvar(ContabilizaConsulta entity, CancellationToken cancellationToken);
        Task<IEnumerable<ContabilizaConsulta>> ObterFiltro(Guid clienteId, Guid produtoId, string dataDe, string dataAte, CancellationToken cancellationToken);
        Task<bool> DeletarPorId(Guid id, CancellationToken cancellationToken);
    }

}
