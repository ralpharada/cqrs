using Tellus.Domain.Models;

namespace Tellus.Domain.Interfaces
{
    public interface IUsuarioLogadoRepository
    {
        Task<bool> ExcluirPorId(Guid id, CancellationToken cancellationToken);
        Task<bool> ExcluirPorUsuarioId(Guid id, CancellationToken cancellationToken);
        Task Adicionar(UsuarioLogado usuarioLogado, CancellationToken cancellationToken);
        Task<List<UsuarioLogado>> ObterPorCliente(Guid clienteId, int currentPage, int pageSize, CancellationToken cancellationToken);
        Task<UsuarioLogado> ObterPorUsuarioId(Guid usuarioId, CancellationToken cancellationToken);
        Task<bool> Atualizar(UsuarioLogado entity, CancellationToken cancellationToken);
        Task<long> CountPorCliente(Guid clienteId, CancellationToken cancellationToken);
        Task ExcluirVencidosPorClienteId(Guid id, CancellationToken cancellationToken);
    }
}
