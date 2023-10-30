using Tellus.Domain.Interfaces;
using Tellus.Domain.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Tellus.Infra.Repositories
{
    public class ProdutoRepository : IProdutoRepository
    {
        private readonly IMongoCollection<Produto> _collection;
        public ProdutoRepository(IOptions<DbSettings> _dbSettings)
        {
            var mongoClient = new MongoClient(_dbSettings.Value.ConnectionString);
            var mongoDataBase = mongoClient.GetDatabase(_dbSettings.Value.DatabaseName);
            _collection = mongoDataBase.GetCollection<Produto>(nameof(Produto));
        }
        public async Task<ReplaceOneResult> Salvar(Produto entity, CancellationToken cancellationToken)
        {
            var filter = new FilterDefinitionBuilder<Produto>().Where(x => x.Id == entity.Id);
            return await _collection.ReplaceOneAsync(filter, entity, new ReplaceOptions { IsUpsert = true }, cancellationToken);
        }
        public async Task<bool> DeletarPorId(Guid id, CancellationToken cancellationToken)
        {
            var filter = new FilterDefinitionBuilder<Produto>().Where(x => x.Id == id);
            var result = await _collection.DeleteOneAsync(filter, cancellationToken);
            return result.DeletedCount > 0;
        }
        public async Task<Produto> ObterPorId(Guid id, CancellationToken cancellationToken)
        {
            return await _collection.Find(x => x.Id == id).FirstOrDefaultAsync(cancellationToken);

        }
        public async Task<IEnumerable<Produto>> ObterTodos(CancellationToken cancellationToken)
        {
            return await _collection.Find(x =>true).SortBy(x=>x.Titulo).ToListAsync(cancellationToken);
        }

        public async Task<List<Produto>> ObterPorIds(List<Guid> ids, CancellationToken cancellationToken)
        {
            return await _collection.Find(x => ids.Contains(x.Id)).ToListAsync(cancellationToken);
        }
    }
}
