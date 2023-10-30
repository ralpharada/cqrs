using Tellus.Domain.Interfaces;
using Tellus.Domain.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Tellus.Infra.Repositories
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly IMongoCollection<Usuario> _usuarioCollection;
        public UsuarioRepository(IOptions<DbSettings> _dbSettings)
        {
            var mongoClient = new MongoClient(_dbSettings.Value.ConnectionString);
            var mongoDataBase = mongoClient.GetDatabase(_dbSettings.Value.DatabaseName);
            _usuarioCollection = mongoDataBase.GetCollection<Usuario>(nameof(Usuario));
        }
        public async Task<ReplaceOneResult> Salvar(Usuario entity, CancellationToken cancellationToken)
        {
            var filter = new FilterDefinitionBuilder<Usuario>().Where(x => x.Id == entity.Id);
            return await _usuarioCollection.ReplaceOneAsync(filter, entity, new ReplaceOptions { IsUpsert = true }, cancellationToken);
        }
        public async Task<bool> AtualizarHashEsqueciSenha(Usuario entity, CancellationToken cancellationToken)
        {
            var filter = new FilterDefinitionBuilder<Usuario>().Where(x => x.Id == entity.Id);
            var result = await _usuarioCollection.ReplaceOneAsync(filter, entity, new ReplaceOptions { IsUpsert = false }, cancellationToken);
            return result.ModifiedCount > 0;
        }

        public async Task<bool> AtualizarSenha(Usuario entity, CancellationToken cancellationToken)
        {
            var filter = new FilterDefinitionBuilder<Usuario>().Where(x => x.Id == entity.Id);
            var result = await _usuarioCollection.ReplaceOneAsync(filter, entity, new ReplaceOptions { IsUpsert = false }, cancellationToken);
            return result.ModifiedCount > 0;
        }

        public async Task<bool> DeletarPorId(Guid id, Guid clienteId, CancellationToken cancellationToken)
        {
            var filter = new FilterDefinitionBuilder<Usuario>().Where(x => x.Id == id && x.ClienteId == clienteId);
            Usuario entity = new()
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

        public async Task<Usuario> ObterPorEmail(string email, CancellationToken cancellationToken)
        {
            return await _usuarioCollection.Find(x => x.Email.ToLower() == email.ToLower() && x.Excluido == null).FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<Usuario> ObterPorEmailCadastroAtivo(string email, CancellationToken cancellationToken)
        {
            return await _usuarioCollection.Find(x => x.Email.ToLower() == email.ToLower() && x.Excluido == null && x.Status).FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<Usuario> ObterPorHashAtivacaoCadastro(string hash, CancellationToken cancellationToken)
        {
            return await _usuarioCollection.Find(x => x.HashAtivacaoCadastro == hash && x.DataAtivacaoCadastro == null && x.DataValidadeAtivacaoCadastro.Value >= DateTime.UtcNow && x.Excluido == null).FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<Usuario> ObterPorHashEsqueciSenha(string hash, CancellationToken cancellationToken)
        {
            return await _usuarioCollection.Find(x => x.HashEsqueciSenha == hash && x.DataValidadeEsqueciSenha.Value >= DateTime.UtcNow && x.Excluido == null).FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<Usuario> ObterPorId(Guid id, CancellationToken cancellationToken)
        {
            return await _usuarioCollection.Find(x => x.Id == id && x.Excluido == null).FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<IEnumerable<Usuario>> ObterTodosPorClienteId(Guid clienteId, string filtro, int currentPage, int pageSize, CancellationToken cancellationToken)
        {
            currentPage--;
            if (currentPage < 0)
                currentPage = 0;
            return await _usuarioCollection.Find(x => x.ClienteId == clienteId && x.Excluido == null && x.Nome.ToLower().Contains(filtro.ToLower())).Skip(currentPage * pageSize).Limit(pageSize).ToListAsync(cancellationToken);
        }
        public async Task<IEnumerable<Usuario>> ObterTodosCompleto(Guid clienteId, CancellationToken cancellationToken)
        {
            return await _usuarioCollection.Find(x => x.ClienteId == clienteId && x.Excluido == null).ToListAsync(cancellationToken);
        }
        public async Task<bool> ExisteUsuario(string email, CancellationToken cancellationToken)
        {
            return await _usuarioCollection.CountDocumentsAsync(x => x.Email.ToLower() == email.ToLower() && x.Excluido == null, null, cancellationToken) > 0;
        }
        public List<Usuario> ObterTodosPorIds(List<Guid> ids, Guid clienteId)
        {
            return (from i in _usuarioCollection.AsQueryable().AsEnumerable()
                    where ids.Any(x => x == i.Id) && i.ClienteId == clienteId
                    select i).ToList();
        }
        public async Task<long> Count(Guid clienteId, string filtro, CancellationToken cancellationToken)
        {
            var filter = new FilterDefinitionBuilder<Usuario>().Where(x => x.ClienteId == clienteId && x.Excluido == null && x.Nome.ToLower().Contains(filtro.ToLower()));
            return await _usuarioCollection.CountDocumentsAsync(filter, null, cancellationToken);
        }
    }
}
