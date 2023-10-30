using Tellus.Domain.Interfaces;
using Tellus.Domain.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Tellus.Infra.Repositories
{
    public class LogRepository : ILogRepository
    {
        private readonly IMongoCollection<Log> _collection;
        public LogRepository(IOptions<DbSettings> _dbSettings)
        {
            var mongoClient = new MongoClient(_dbSettings.Value.ConnectionString);
            var mongoDataBase = mongoClient.GetDatabase(_dbSettings.Value.DatabaseName);
            _collection = mongoDataBase.GetCollection<Log>(nameof(Log));
        }
        public async Task<IEnumerable<Log>> Listar(string dataDe, string dataAte, CancellationToken cancellationToken)
        {
            var filtro = Builders<Log>.Filter.Where(x =>true);
            //filtro = filtro & Builders<ContabilizaConsulta>.Filter.Eq(x => x.ProdutoId, produtoId);
            //if (!string.IsNullOrEmpty(dataDe))
            //{
            //    var dataInicio = Convert.ToDateTime(dataDe);
            //    filtro = filtro & Builders<Log>.Filter.Gte(x => x.Data, dataInicio);
            //}

            //if (!string.IsNullOrEmpty(dataAte))
            //{
            //    var dataFim = Convert.ToDateTime(dataAte);
            //    filtro = filtro & Builders<Log>.Filter.Lte(x => x.Data, dataFim);
            //}

            return await _collection.Find(filtro).SortByDescending(x=>x.Id).ToListAsync(cancellationToken);
        }
    }
}
