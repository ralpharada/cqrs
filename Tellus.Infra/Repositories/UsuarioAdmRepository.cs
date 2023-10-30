using Tellus.Domain.Interfaces;
using Tellus.Domain.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Tellus.Infra.Repositories
{
    public class UsuarioAdmRepository : IUsuarioAdmRepository
    {
        private readonly IMongoCollection<UsuarioAdm> _usuarioCollection;
        public UsuarioAdmRepository(IOptions<DbSettings> _dbSettings)
        {
            var mongoClient = new MongoClient(_dbSettings.Value.ConnectionString);
            var mongoDataBase = mongoClient.GetDatabase(_dbSettings.Value.DatabaseName);
            _usuarioCollection = mongoDataBase.GetCollection<UsuarioAdm>(nameof(UsuarioAdm));
        }
        public async Task<ReplaceOneResult> Salvar(UsuarioAdm entity, CancellationToken cancellationToken)
        {
            var filter = new FilterDefinitionBuilder<UsuarioAdm>().Where(x => x.Id == entity.Id);
            return await _usuarioCollection.ReplaceOneAsync(filter, entity, new ReplaceOptions { IsUpsert = true }, cancellationToken);
        }
        public async Task<bool> AtualizarHashEsqueciSenha(UsuarioAdm entity, CancellationToken cancellationToken)
        {
            var filter = new FilterDefinitionBuilder<UsuarioAdm>().Where(x => x.Id == entity.Id);
            var result = await _usuarioCollection.ReplaceOneAsync(filter, entity, new ReplaceOptions { IsUpsert = false }, cancellationToken);
            return result.ModifiedCount > 0;
        }

        public async Task<bool> AtualizarSenha(UsuarioAdm entity, CancellationToken cancellationToken)
        {
            var filter = new FilterDefinitionBuilder<UsuarioAdm>().Where(x => x.Id == entity.Id);
            var result = await _usuarioCollection.ReplaceOneAsync(filter, entity, new ReplaceOptions { IsUpsert = false }, cancellationToken);
            return result.ModifiedCount > 0;
        }

        public async Task<bool> DeletarPorId(Guid id, CancellationToken cancellationToken)
        {
            var filter = new FilterDefinitionBuilder<UsuarioAdm>().Where(x => x.Id == id);
            UsuarioAdm entity = new()
            {
                Id = id,
                Excluido = true,
                DataExclusao = DateTime.UtcNow
            };
            var result = await _usuarioCollection.ReplaceOneAsync(filter, entity, new ReplaceOptions { IsUpsert = false }, cancellationToken);
            return result.ModifiedCount > 0;
        }

        public async Task<bool> ExisteCadastroPorEmail(string email)
        {
            return await _usuarioCollection.Find(x => x.Email.ToLower() == email.ToLower() && x.Excluido == null).CountDocumentsAsync() > 0;
        }

        public async Task<UsuarioAdm> ObterPorEmail(string email, CancellationToken cancellationToken)
        {
            return await _usuarioCollection.Find(x => x.Email.ToLower() == email.ToLower() && x.Excluido == null).FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<UsuarioAdm> ObterPorEmailCadastroAtivo(string email, CancellationToken cancellationToken)
        {
            return await _usuarioCollection.Find(x => x.Email.ToLower() == email.ToLower() && x.Excluido == null && x.Status).FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<UsuarioAdm> ObterPorHashAtivacaoCadastro(string hash, CancellationToken cancellationToken)
        {
            return await _usuarioCollection.Find(x => x.HashAtivacaoCadastro == hash && x.DataAtivacaoCadastro == null && x.DataValidadeAtivacaoCadastro.Value >= DateTime.UtcNow && x.Excluido == null).FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<UsuarioAdm> ObterPorHashEsqueciSenha(string hash, CancellationToken cancellationToken)
        {
            return await _usuarioCollection.Find(x => x.HashEsqueciSenha == hash && x.DataValidadeEsqueciSenha.Value >= DateTime.UtcNow && x.Excluido == null).FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<UsuarioAdm> ObterPorId(Guid id, CancellationToken cancellationToken)
        {
            return await _usuarioCollection.Find(x => x.Id == id && x.Excluido == null).FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<IEnumerable<UsuarioAdm>> ObterTodos(string filtro, int currentPage, int pageSize, CancellationToken cancellationToken)
        {
            currentPage--;
            if (currentPage < 0)
                currentPage = 0;
            return await _usuarioCollection.Find(x => x.Excluido == null && (x.Nome.ToLower().Contains(filtro) || x.Email.ToLower().Contains(filtro))).Skip(currentPage * pageSize).Limit(pageSize).ToListAsync(cancellationToken);
        }
        public async Task<bool> ExisteUsuario(string email, CancellationToken cancellationToken)
        {
            return await _usuarioCollection.CountDocumentsAsync(x => x.Email.ToLower() == email.ToLower() && x.Excluido == null, null, cancellationToken) > 0;
        }
        public async Task<long> Count(CancellationToken cancellationToken)
        {
            var filter = new FilterDefinitionBuilder<UsuarioAdm>().Where(x => x.Excluido == null);
            return await _usuarioCollection.CountDocumentsAsync(filter, null, cancellationToken);
        }
    }
}
