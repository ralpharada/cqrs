using Tellus.Domain.Interfaces;
using Tellus.Domain.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Tellus.Infra.Repositories
{
    public class GrupoUsuarioRepository : IGrupoUsuarioRepository
    {
        private readonly IMongoCollection<GrupoUsuario> _grupoUsuarioCollection;
        public GrupoUsuarioRepository(IOptions<DbSettings> _dbSettings)
        {
            var mongoClient = new MongoClient(_dbSettings.Value.ConnectionString);
            var mongoDataBase = mongoClient.GetDatabase(_dbSettings.Value.DatabaseName);
            _grupoUsuarioCollection = mongoDataBase.GetCollection<GrupoUsuario>(nameof(GrupoUsuario));
        }
        public async Task<ReplaceOneResult> Salvar(GrupoUsuario entity, CancellationToken cancellationToken)
        {
            var filter = new FilterDefinitionBuilder<GrupoUsuario>().Where(x => x.Id == entity.Id && x.ClienteId == entity.ClienteId);
            return await _grupoUsuarioCollection.ReplaceOneAsync(filter, entity, new ReplaceOptions { IsUpsert = true }, cancellationToken);
        }
        public async Task<bool> DeletarPorId(Guid id, Guid clienteId, CancellationToken cancellationToken)
        {
            var filter = new FilterDefinitionBuilder<GrupoUsuario>().Where(x => x.Id == id && x.ClienteId == clienteId);
            var result = await _grupoUsuarioCollection.DeleteOneAsync(filter, cancellationToken);
            return result.DeletedCount > 0;
        }
        public async Task<GrupoUsuario> ObterPorId(Guid id, Guid clienteId, CancellationToken cancellationToken)
        {
            return await _grupoUsuarioCollection.Find(x => x.Id == id && x.ClienteId == clienteId).FirstOrDefaultAsync(cancellationToken);
        }
        public async Task<IEnumerable<GrupoUsuario>> ObterTodosPorClienteId(Guid clienteId, string filtro, int currentPage, int pageSize, CancellationToken cancellationToken)
        {
            currentPage--;
            if (currentPage < 0)
                currentPage = 0;
            return await _grupoUsuarioCollection.Find(x => x.ClienteId == clienteId && x.Nome.ToLower().Contains(filtro)).Skip(currentPage * pageSize).Limit(pageSize).ToListAsync(cancellationToken);

        }
        public async Task<IEnumerable<GrupoUsuario>> ObterTodos(Guid clienteId, CancellationToken cancellationToken)
        {
            return await _grupoUsuarioCollection.Find(x => x.ClienteId == clienteId).ToListAsync(cancellationToken);

        }
        public async Task<bool> BloquearPorId(Guid id, Guid clienteId, CancellationToken cancellationToken)
        {
            var grupoUsuario = await ObterPorId(id, clienteId, cancellationToken);
            var filter = new FilterDefinitionBuilder<GrupoUsuario>().Where(x => x.Id == id && x.ClienteId == clienteId);
            var result = await _grupoUsuarioCollection.UpdateOneAsync(filter, Builders<GrupoUsuario>.Update.Set(x => x.Bloqueado, !grupoUsuario.Bloqueado));
            return result.ModifiedCount > 0;
        }
        public async Task<long> Count(Guid clienteId, CancellationToken cancellationToken)
        {
            var filter = new FilterDefinitionBuilder<GrupoUsuario>().Where(x => x.ClienteId == clienteId);
            return await _grupoUsuarioCollection.CountDocumentsAsync(filter, null, cancellationToken);
        }
        public IEnumerable<GrupoUsuario> ObterTodosPorUsuarioId(Guid usuarioId, Guid clienteId)
        {
            return (from i in _grupoUsuarioCollection.AsQueryable().AsEnumerable()
                    where i.UsuarioIds != null && i.UsuarioIds.Any(x => x == usuarioId) && i.ClienteId == clienteId && !i.Bloqueado && i.Status
                    select i).ToList();
        }
    }
}
