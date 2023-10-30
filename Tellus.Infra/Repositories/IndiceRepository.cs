using Tellus.Domain.Interfaces;
using Tellus.Domain.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Tellus.Infra.Repositories
{
    public class IndiceRepository : IIndiceRepository
    {
        private readonly IMongoCollection<Indice> _indiceCollection;
        public IndiceRepository(IOptions<DbSettings> _dbSettings)
        {
            var mongoClient = new MongoClient(_dbSettings.Value.ConnectionString);
            var mongoDataBase = mongoClient.GetDatabase(_dbSettings.Value.DatabaseName);
            _indiceCollection = mongoDataBase.GetCollection<Indice>(nameof(Indice));
        }
        public async Task<ReplaceOneResult> Salvar(Indice entity, CancellationToken cancellationToken)
        {
            var filter = new FilterDefinitionBuilder<Indice>().Where(x => x.Id == entity.Id && x.ClienteId == entity.ClienteId);
            return await _indiceCollection.ReplaceOneAsync(filter, entity, new ReplaceOptions { IsUpsert = true }, cancellationToken);
        }
        public async Task<bool> DeletarPorId(Guid id, Guid clienteId, CancellationToken cancellationToken)
        {
            var filter = new FilterDefinitionBuilder<Indice>().Where(x => x.Id == id && x.ClienteId == clienteId);
            var result = await _indiceCollection.DeleteOneAsync(filter, cancellationToken);
            return result.DeletedCount > 0;
        }
        public async Task<Indice> ObterPorId(Guid id, Guid clienteId, CancellationToken cancellationToken)
        {
            return await _indiceCollection.Find(x => x.Id == id && x.ClienteId == clienteId).FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<IEnumerable<Indice>> ObterTodosPorClienteId(Guid clienteId, CancellationToken cancellationToken)
        {
            return await _indiceCollection.Find(x => x.ClienteId == clienteId).ToListAsync(cancellationToken);

        }
        public List<Indice> ObterTodosPorIds(List<Guid> ids, Guid clienteId)
        {
            return (from i in _indiceCollection.AsQueryable().AsEnumerable()
                          where ids.Any(x => x == i.Id) && i.ClienteId == clienteId
                          select i).ToList();
        }
    }
}
