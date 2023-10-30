using Tellus.Domain.Models;
using MongoDB.Driver;

namespace Tellus.Domain.Interfaces
{
    public interface IDocumentoRepository
    {
        Task<Documento> ObterPorId(Guid id, Guid clienteId, CancellationToken cancellationToken);
        Task<ReplaceOneResult> Salvar(Documento entity, CancellationToken cancellationToken);
        Task<bool> DeletarPorId(Guid id, Guid clienteId, CancellationToken cancellationToken);
        Task<bool> DeletarPorIds(List<Guid> ids, Guid clienteId, CancellationToken cancellationToken);
        Task<List<Documento>> Pesquisar(Guid clienteId, Guid tipoDocumentoId, string dataImportacaoDe, string dataImportacaoAte, List<IndiceValor> indiceValores, int currentPage, int pageSize, CancellationToken cancellationToken);
        Task<long> Count(Guid clienteId, Guid tipoDocumentoId, string dataImportacaoDe, string dataImportacaoAte, List<IndiceValor> indiceValores, CancellationToken cancellationToken);
        Task<List<Documento>> ObterPorIds(List<Guid> ids, Guid clienteId, CancellationToken cancellationToken);
        Task<List<Documento>> ObterTotalArmazenamento(Guid clienteId, CancellationToken cancellationToken);
        Task<List<Documento>> PesquisaRapida(Guid clienteId, Guid tipoDocumentoId, string pesquisa, int currentPage, int pageSize, CancellationToken cancellationToken);
        Task<long> CountPesquisaRapida(Guid clienteId, Guid tipoDocumentoId, string pesquisa, CancellationToken cancellationToken);
        Task<List<Guid>> PesquisaRapidaTiposDocumento(Guid clienteId, string pesquisa);
        Task<List<Documento>> PesquisaDocumentosExcluidos(Guid clienteId, int currentPage, int pageSize, CancellationToken cancellationToken);
        Task<long> CountPesquisaDocumentosExcluidos(Guid clienteId, CancellationToken cancellationToken);
        Task<long> CountTotalDocumentos(Guid clienteId, CancellationToken cancellationToken);
        Task<long> CountTotalDocumentosPorUsuario(Guid usuarioId, CancellationToken cancellationToken);
        Task<List<Documento>> ObterPorCliente(Guid clienteId, CancellationToken cancellationToken);
        Task<List<Documento>> DocumentoPorClienteDataDe(Guid clienteId, string dataImportacaoAte, CancellationToken cancellationToken);
        Task<bool> ExisteDocumentoAssociadoTipoDocumento(Guid tipoDOcumentoId, Guid clienteId, CancellationToken cancellationToken);
        Task<List<Documento>> ObterPorUsuario(Guid usuarioId, CancellationToken cancellationToken);
        Task<List<Documento>> DocumentoPorUsuarioDataDe(Guid usuarioId, string dataImportacaoAte, CancellationToken cancellationToken);

    }

}
