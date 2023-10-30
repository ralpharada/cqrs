using Tellus.Domain.Interfaces;
using Tellus.Domain.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Tellus.Infra.Repositories
{
    public class LogClienteRepository : ILogClienteRepository
    {
        private readonly IMongoCollection<LogCliente> documentoCollection;
        public LogClienteRepository(IOptions<DbSettings> _dbSettings)
        {
            var mongoClient = new MongoClient(_dbSettings.Value.ConnectionString);
            var mongoDataBase = mongoClient.GetDatabase(_dbSettings.Value.DatabaseName);
            documentoCollection = mongoDataBase.GetCollection<LogCliente>(nameof(LogCliente));
        }
        public async Task<List<LogCliente>> ObterTodosPorClienteId(Guid clienteId, CancellationToken cancellationToken)
        {
            return await documentoCollection.Find(x => x.ClienteId == clienteId).SortByDescending(x => x.DataRegistro).ToListAsync(cancellationToken);

        }
        public async Task<ReplaceOneResult> Salvar(LogCliente entity, CancellationToken cancellationToken)
        {
            var filter = new FilterDefinitionBuilder<LogCliente>().Where(x => x.Id == entity.Id && x.ClienteId == entity.ClienteId);
            return await documentoCollection.ReplaceOneAsync(filter, entity, new ReplaceOptions { IsUpsert = true }, cancellationToken);
        }
    }
}
