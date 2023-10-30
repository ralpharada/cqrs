using Tellus.Domain.Interfaces;
using Tellus.Domain.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Tellus.Infra.Repositories
{
    public class RefreshTokenRepository : IRefreshTokenRepository
    {
        private readonly IMongoCollection<RefreshToken> _refreshTokenCollection;
        public RefreshTokenRepository(IOptions<DbSettings> _dbSettings)
        {
            var mongoClient = new MongoClient(_dbSettings.Value.ConnectionString);
            var mongoDataBase = mongoClient.GetDatabase(_dbSettings.Value.DatabaseName);
            _refreshTokenCollection = mongoDataBase.GetCollection<RefreshToken>(nameof(RefreshToken));
        }
        public async Task AtualizarPorClienteId(RefreshToken refreshToken)
        {
            var result = await _refreshTokenCollection.DeleteOneAsync(x => x.ClienteId == refreshToken.ClienteId);
            await _refreshTokenCollection.InsertOneAsync(refreshToken);
        }

        public async Task AtualizarPorUsuarioId(RefreshToken refreshToken)
        {
            var result = await _refreshTokenCollection.DeleteOneAsync(x => x.UsuarioId == refreshToken.UsuarioId && x.ETipoUsuario == refreshToken.ETipoUsuario);
            await _refreshTokenCollection.InsertOneAsync(refreshToken);
        }

        public async Task<RefreshToken> ObterPorChaveCliente(string refreshToken)
        {
            return await _refreshTokenCollection.Find(x => x.Token == refreshToken && x.ClienteId.HasValue).FirstOrDefaultAsync();
        }

        public async Task<RefreshToken> ObterPorChaveUsuario(string refreshToken)
        {
            return await _refreshTokenCollection.Find(x => x.Token == refreshToken && x.UsuarioId.HasValue).FirstOrDefaultAsync();
        }
    }
}
