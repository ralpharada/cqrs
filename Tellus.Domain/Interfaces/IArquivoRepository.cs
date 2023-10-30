using Tellus.Domain.Models;

namespace Tellus.Domain.Interfaces
{
    public interface IArquivoRepository
    {
        Task<IEnumerable<Arquivo>> ObterTodosPorProdutoId(int produtoId, CancellationToken cancellationToken);
        Task<Arquivo> ObterPorId(int id, CancellationToken cancellationToken);
        Task<bool> Adicionar(Arquivo entity, CancellationToken cancellationToken);
        Task<bool> Atualizar(Arquivo entity, CancellationToken cancellationToken);
        Task<bool> DeletarPorId(int id, CancellationToken cancellationToken);
        Task<int> ContarPorProdutoId(int produtoId, CancellationToken cancellationToken);
    }
}
