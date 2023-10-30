using Tellus.Domain.Interfaces;
using Tellus.Domain.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Tellus.Infra.Repositories
{
    public class PermissaoRepository : IPermissaoRepository
    {
        private readonly IMongoCollection<Permissao> _permissaoCollection;
        public PermissaoRepository(IOptions<DbSettings> _dbSettings)
        {
            var mongoClient = new MongoClient(_dbSettings.Value.ConnectionString);
            var mongoDataBase = mongoClient.GetDatabase(_dbSettings.Value.DatabaseName);
            _permissaoCollection = mongoDataBase.GetCollection<Permissao>(nameof(Permissao));
        }
        public async Task<ReplaceOneResult> Salvar(Permissao entity, CancellationToken cancellationToken)
        {
            var filter = new FilterDefinitionBuilder<Permissao>().Where(x => x.Id == entity.Id);
            return await _permissaoCollection.ReplaceOneAsync(filter, entity, new ReplaceOptions { IsUpsert = true }, cancellationToken);
        }
        public async Task<bool> DeletarPorId(Guid id, CancellationToken cancellationToken)
        {
            var filter = new FilterDefinitionBuilder<Permissao>().Where(x => x.Id == id);
            var result = await _permissaoCollection.DeleteOneAsync(filter, cancellationToken);
            return result.DeletedCount > 0;
        }
        public async Task<Permissao> ObterPorId(Guid id, CancellationToken cancellationToken)
        {
            return await _permissaoCollection.Find(x => x.Id == id).FirstOrDefaultAsync(cancellationToken);

        }
        public async Task<IEnumerable<Permissao>> ObterTodos(Guid? vinculoId, CancellationToken cancellationToken)
        {
            var filtro = vinculoId.HasValue ? Builders<Permissao>.Filter.Eq(p => p.VinculoId, vinculoId.Value) : Builders<Permissao>.Filter.Empty;
            return await _permissaoCollection.Find(filtro).ToListAsync(cancellationToken);
        }
        public List<Permissao> ObterTodosPorIds(List<Guid> ids)
        {
            return (from i in _permissaoCollection.AsQueryable().AsEnumerable()
                    where ids.Any(x => x == i.Id)
                    select i).ToList();
        }
    }
}
