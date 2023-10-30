using Tellus.Domain.Interfaces;
using Tellus.Domain.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace Tellus.Infra.Repositories
{
    public class LogDocumentoRepository : ILogDocumentoRepository
    {
        private readonly IMongoCollection<LogDocumento> documentoCollection;
        public LogDocumentoRepository(IOptions<DbSettings> _dbSettings)
        {
            var mongoClient = new MongoClient(_dbSettings.Value.ConnectionString);
            var mongoDataBase = mongoClient.GetDatabase(_dbSettings.Value.DatabaseName);
            documentoCollection = mongoDataBase.GetCollection<LogDocumento>(nameof(LogDocumento));
        }
        public async Task<List<LogDocumento>> ObterTodosPorDocumentoId(Guid id, Guid clienteId, CancellationToken cancellationToken)
        {
            return await documentoCollection.Find(x => x.ClienteId == clienteId && x.DocumentoId == id).SortByDescending(x=>x.DataRegistro).ToListAsync(cancellationToken);

        }
        public async Task<ReplaceOneResult> Salvar(LogDocumento entity, CancellationToken cancellationToken)
        {
            var filter = new FilterDefinitionBuilder<LogDocumento>().Where(x => x.DocumentoId == entity.Id && x.ClienteId == entity.ClienteId);
            return await documentoCollection.ReplaceOneAsync(filter, entity, new ReplaceOptions { IsUpsert = true }, cancellationToken);
        }
    }
}
