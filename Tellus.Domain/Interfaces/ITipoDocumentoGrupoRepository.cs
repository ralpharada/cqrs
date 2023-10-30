using Tellus.Domain.Models;
using MongoDB.Driver;

namespace Tellus.Domain.Interfaces
{
    public interface ITipoDocumentoGrupoRepository
    {
        Task<TipoDocumentoGrupo> ObterPorId(Guid id, Guid clienteId, CancellationToken cancellationToken);
        Task<ReplaceOneResult> Salvar(TipoDocumentoGrupo entity, CancellationToken cancellationToken);
        Task<bool> DeletarPorId(Guid id, Guid clienteId, CancellationToken cancellationToken);
        Task<long> Count(Guid clienteId, CancellationToken cancellationToken);
        Task<IEnumerable<TipoDocumentoGrupo>> ObterTodosPorClienteId(Guid clienteId, CancellationToken cancellationToken);
        Task<IEnumerable<TipoDocumentoGrupo>> ObterTodos(Guid clienteId, string filtro, int currentPage, int pageSize, CancellationToken cancellationToken);
        Task<IEnumerable<TipoDocumentoGrupo>> ObterTodosCompleto(Guid clienteId, CancellationToken cancellationToken);
        Task<IEnumerable<TipoDocumentoGrupo>> ObterTodosCompletoAtivo(Guid clienteId, CancellationToken cancellationToken);
    }

}
