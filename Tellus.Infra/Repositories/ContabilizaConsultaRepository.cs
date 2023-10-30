using Tellus.Domain.Interfaces;
using Tellus.Domain.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Tellus.Infra.Repositories
{
    public class ContabilizaConsultaRepository : IContabilizaConsultaRepository
    {
        private readonly IMongoCollection<ContabilizaConsulta> _collection;
        public ContabilizaConsultaRepository(IOptions<DbSettings> _dbSettings)
        {
            var mongoClient = new MongoClient(_dbSettings.Value.ConnectionString);
            var mongoDataBase = mongoClient.GetDatabase(_dbSettings.Value.DatabaseName);
            _collection = mongoDataBase.GetCollection<ContabilizaConsulta>(nameof(ContabilizaConsulta));
        }
        public async Task<ReplaceOneResult> Salvar(ContabilizaConsulta entity, CancellationToken cancellationToken)
        {
            var filter = new FilterDefinitionBuilder<ContabilizaConsulta>().Where(x => x.Id == entity.Id);
            return await _collection.ReplaceOneAsync(filter, entity, new ReplaceOptions { IsUpsert = true }, cancellationToken);
        }
        public async Task<IEnumerable<ContabilizaConsulta>> ObterFiltro(Guid clienteId, Guid produtoId, string dataDe, string dataAte, CancellationToken cancellationToken)
        {
            var filtro = Builders<ContabilizaConsulta>.Filter.Eq(x => x.ClienteId, clienteId);
            filtro = filtro & Builders<ContabilizaConsulta>.Filter.Eq(x => x.ProdutoId, produtoId);
            if (!string.IsNullOrEmpty(dataDe))
            {
                var dataInicio = Convert.ToDateTime(dataDe);
                filtro = filtro & Builders<ContabilizaConsulta>.Filter.Gte(x => x.Data, dataInicio);
            }

            if (!string.IsNullOrEmpty(dataAte))
            {
                var dataFim = Convert.ToDateTime(dataAte);
                filtro = filtro & Builders<ContabilizaConsulta>.Filter.Lte(x => x.Data, dataFim);
            }

            return await _collection.Find(filtro).ToListAsync(cancellationToken);
        }
        public async Task<bool> DeletarPorId(Guid id, CancellationToken cancellationToken)
        {
            var filter = new FilterDefinitionBuilder<ContabilizaConsulta>().Where(x => x.Id == id);
            var result = await _collection.DeleteOneAsync(filter, null, cancellationToken);
            return result.DeletedCount > 0;
        }
    }
}
