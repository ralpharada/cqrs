using Tellus.Domain.Interfaces;
using Tellus.Domain.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Tellus.Infra.Repositories
{
    public class UsuarioLogadoRepository : IUsuarioLogadoRepository
    {
        private readonly IMongoCollection<UsuarioLogado> _usuarioLogadoCollection;
        public UsuarioLogadoRepository(IOptions<DbSettings> _dbSettings)
        {
            var mongoClient = new MongoClient(_dbSettings.Value.ConnectionString);
            var mongoDataBase = mongoClient.GetDatabase(_dbSettings.Value.DatabaseName);
            _usuarioLogadoCollection = mongoDataBase.GetCollection<UsuarioLogado>(nameof(UsuarioLogado));
        }
        public async Task<bool> ExcluirPorId(Guid id, CancellationToken cancellationToken)
        {
            var consulta = await _usuarioLogadoCollection.FindAsync(x => x.Id == id && x.Excluido == null, null, cancellationToken);
            var usuarioLogado = consulta.FirstOrDefault();
            if (usuarioLogado != null)
            {
                usuarioLogado.Excluido = DateTime.UtcNow;
                var filter = new FilterDefinitionBuilder<UsuarioLogado>().Where(x => x.Id == usuarioLogado.Id);
                var result = await _usuarioLogadoCollection.ReplaceOneAsync(filter, usuarioLogado, new ReplaceOptions { IsUpsert = false }, cancellationToken);
                return result.ModifiedCount > 0;
            }
            return false;
        }
        public async Task<bool> ExcluirPorUsuarioId(Guid id, CancellationToken cancellationToken)
        {
            var consulta = await _usuarioLogadoCollection.FindAsync(x => x.UsuarioId == id && x.Excluido == null, null, cancellationToken);
            var usuarioLogado = consulta.FirstOrDefault();
            if (usuarioLogado != null)
            {
                usuarioLogado.Excluido = DateTime.UtcNow;
                var filter = new FilterDefinitionBuilder<UsuarioLogado>().Where(x => x.Id == usuarioLogado.Id);
                var result = await _usuarioLogadoCollection.ReplaceOneAsync(filter, usuarioLogado, new ReplaceOptions { IsUpsert = false }, cancellationToken);
                return result.ModifiedCount > 0;
            }
            return true;
        }
        public async Task ExcluirVencidosPorClienteId(Guid id, CancellationToken cancellationToken)
        {
            var consulta = await _usuarioLogadoCollection.FindAsync(x => x.ClienteId == id && x.Excluido == null, null, cancellationToken);
            foreach (var usuarioLogado in await consulta.ToListAsync(cancellationToken))
            {
                if(usuarioLogado.UltimaRequisicao.AddMinutes(15)< DateTime.UtcNow)
                {
                    usuarioLogado.Excluido = DateTime.UtcNow;
                    var filter = new FilterDefinitionBuilder<UsuarioLogado>().Where(x => x.Id == usuarioLogado.Id);
                    var result = await _usuarioLogadoCollection.ReplaceOneAsync(filter, usuarioLogado, new ReplaceOptions { IsUpsert = false }, cancellationToken);
                }
              
            }
        }
        public async Task Adicionar(UsuarioLogado usuarioLogado, CancellationToken cancellationToken)
        {
            await _usuarioLogadoCollection.InsertOneAsync(usuarioLogado, cancellationToken);
        }

        public async Task<List<UsuarioLogado>> ObterPorCliente(Guid clienteId, int currentPage, int pageSize, CancellationToken cancellationToken)
        {
            currentPage--;
            if (currentPage < 0)
                currentPage = 0;
            return await _usuarioLogadoCollection.Find(x => x.ClienteId == clienteId && x.Excluido == null).Skip(currentPage * pageSize).Limit(pageSize).ToListAsync(cancellationToken);
        }
        public async Task<long> CountPorCliente(Guid clienteId, CancellationToken cancellationToken)
        {
            var filter = new FilterDefinitionBuilder<UsuarioLogado>().Where(x => x.ClienteId == clienteId && x.Excluido == null);
            return await _usuarioLogadoCollection.CountDocumentsAsync(filter, null, cancellationToken);
        }
        public async Task<UsuarioLogado> ObterPorUsuarioId(Guid usuarioId, CancellationToken cancellationToken)
        {
            var consulta = await _usuarioLogadoCollection.FindAsync(x => x.UsuarioId == usuarioId && x.Excluido == null, null, cancellationToken);
            return await consulta.FirstOrDefaultAsync(cancellationToken);
        }
        public async Task<bool> Atualizar(UsuarioLogado entity, CancellationToken cancellationToken)
        {
            var filter = new FilterDefinitionBuilder<UsuarioLogado>().Where(x => x.Id == entity.Id && x.Excluido == null);
            var result = await _usuarioLogadoCollection.ReplaceOneAsync(filter, entity, new ReplaceOptions { IsUpsert = false }, cancellationToken);
            return result.ModifiedCount > 0;
        }
    }
}
