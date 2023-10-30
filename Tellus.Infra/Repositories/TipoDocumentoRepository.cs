using Tellus.Domain.Interfaces;
using Tellus.Domain.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Tellus.Infra.Repositories
{
    public class TipoDocumentoRepository : ITipoDocumentoRepository
    {
        private readonly IMongoCollection<TipoDocumento> _tipoDocumentoCollection;
        public TipoDocumentoRepository(IOptions<DbSettings> _dbSettings)
        {
            var mongoClient = new MongoClient(_dbSettings.Value.ConnectionString);
            var mongoDataBase = mongoClient.GetDatabase(_dbSettings.Value.DatabaseName);
            _tipoDocumentoCollection = mongoDataBase.GetCollection<TipoDocumento>(nameof(TipoDocumento));
        }
        public async Task<ReplaceOneResult> Salvar(TipoDocumento entity, CancellationToken cancellationToken)
        {
            var filter = new FilterDefinitionBuilder<TipoDocumento>().Where(x => x.Id == entity.Id && x.ClienteId == entity.ClienteId);
            return await _tipoDocumentoCollection.ReplaceOneAsync(filter, entity, new ReplaceOptions { IsUpsert = true }, cancellationToken);
        }
        public async Task<bool> DeletarPorId(Guid id, Guid clienteId, CancellationToken cancellationToken)
        {
            var filter = new FilterDefinitionBuilder<TipoDocumento>().Where(x => x.Id == id && x.ClienteId == clienteId);
            var result = await _tipoDocumentoCollection.DeleteOneAsync(filter, cancellationToken);
            return result.DeletedCount > 0;
        }
        public async Task<TipoDocumento> ObterPorId(Guid id, Guid clienteId, CancellationToken cancellationToken)
        {
            return await _tipoDocumentoCollection.Find(x => x.Id == id && x.ClienteId == clienteId).FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<IEnumerable<TipoDocumento>> ObterTodosPorClienteId(Guid clienteId, string filtro, int currentPage, int pageSize, CancellationToken cancellationToken)
        {
            currentPage--;
            if (currentPage < 0)
                currentPage = 0;
            return await _tipoDocumentoCollection.Find(x => x.ClienteId == clienteId && x.Nome.ToLower().Contains(filtro)).Skip(currentPage * pageSize).Limit(pageSize).ToListAsync(cancellationToken);

        }
        public async Task<IEnumerable<TipoDocumento>> ObterTodosCompleto(Guid clienteId, CancellationToken cancellationToken)
        {
            return await _tipoDocumentoCollection.Find(x => x.ClienteId == clienteId).ToListAsync(cancellationToken);

        }
        public async Task<bool> ExisteAssociacao(Guid indiceId, Guid clienteId, CancellationToken cancellationToken)
        {
            var filter = new FilterDefinitionBuilder<TipoDocumento>().Where(x => x.IndiceIds.Any(i => i == indiceId) && x.ClienteId == clienteId);
            var result = await _tipoDocumentoCollection.Find(filter).FirstOrDefaultAsync(cancellationToken);
            return result != null;
        }
        public List<TipoDocumento> ObterTodosPorIds(List<Guid> ids, Guid clienteId)
        {
            return (from i in _tipoDocumentoCollection.AsQueryable().AsEnumerable()
                    where ids.Any(x => x == i.Id) && i.ClienteId == clienteId
                    select i).ToList();
        }
        public async Task<long> Count(Guid clienteId, CancellationToken cancellationToken)
        {
            var filter = new FilterDefinitionBuilder<TipoDocumento>().Where(x => x.ClienteId == clienteId);
            return await _tipoDocumentoCollection.CountDocumentsAsync(filter, null, cancellationToken);
        }
    }
}
