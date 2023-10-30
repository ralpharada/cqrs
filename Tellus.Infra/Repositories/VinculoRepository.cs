using Tellus.Domain.Interfaces;
using Tellus.Domain.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Tellus.Infra.Repositories
{
    public class VinculoRepository : IVinculoRepository
    {
        private readonly IMongoCollection<Vinculo> _vinculoCollection;
        public VinculoRepository(IOptions<DbSettings> _dbSettings)
        {
            var mongoClient = new MongoClient(_dbSettings.Value.ConnectionString);
            var mongoDataBase = mongoClient.GetDatabase(_dbSettings.Value.DatabaseName);
            _vinculoCollection = mongoDataBase.GetCollection<Vinculo>(nameof(Vinculo));
        }
        public async Task<ReplaceOneResult> Salvar(Vinculo entity, CancellationToken cancellationToken)
        {
            var filter = new FilterDefinitionBuilder<Vinculo>().Where(x => x.Id == entity.Id);
            return await _vinculoCollection.ReplaceOneAsync(filter, entity, new ReplaceOptions { IsUpsert = true }, cancellationToken);
        }
        public async Task<bool> DeletarPorId(Guid id, CancellationToken cancellationToken)
        {
            var filter = new FilterDefinitionBuilder<Vinculo>().Where(x => x.Id == id);
            var result = await _vinculoCollection.DeleteOneAsync(filter, cancellationToken);
            return result.DeletedCount > 0;
        }
        public async Task<Vinculo> ObterPorId(Guid id, CancellationToken cancellationToken)
        {
            return await _vinculoCollection.Find(x => x.Id == id).FirstOrDefaultAsync(cancellationToken);

        }
        public async Task<IEnumerable<Vinculo>> ObterTodos(CancellationToken cancellationToken)
        {
            return await _vinculoCollection.Find(x =>true).SortBy(x=>x.Nome).ToListAsync(cancellationToken);
        }
    }
}
