using Tellus.Domain.Interfaces;
using Tellus.Domain.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Tellus.Infra.Repositories
{
    public class ClienteRepository : IClienteRepository
    {
        private readonly IMongoCollection<Cliente> _clienteCollection;
        public ClienteRepository(IOptions<DbSettings> _dbSettings)
        {
            var mongoClient = new MongoClient(_dbSettings.Value.ConnectionString);
            var mongoDataBase = mongoClient.GetDatabase(_dbSettings.Value.DatabaseName);
            _clienteCollection = mongoDataBase.GetCollection<Cliente>(nameof(Cliente));
        }
        public async Task<ReplaceOneResult> Salvar(Cliente entity, CancellationToken cancellationToken)
        {
            var filter = new FilterDefinitionBuilder<Cliente>().Where(x => x.Id == entity.Id);
            return await _clienteCollection.ReplaceOneAsync(filter, entity, new ReplaceOptions { IsUpsert = true }, cancellationToken);
        }
        public async Task<bool> DeletarPorId(Guid id, CancellationToken cancellationToken)
        {
            var filter = new FilterDefinitionBuilder<Cliente>().Where(x => x.Id == id);
            Cliente entity = new()
            {
                Id = id,
                Excluido = true,
                DataExclusao = DateTime.UtcNow
            };
            var result = await _clienteCollection.ReplaceOneAsync(filter, entity, new ReplaceOptions { IsUpsert = false }, cancellationToken);
            return result.ModifiedCount > 0;
        }
        public async Task<bool> ExisteCadastroPorEmail(string email)
        {
            return await _clienteCollection.Find(x => x.Email.ToLower() == email.ToLower() && x.Excluido == null).CountDocumentsAsync() > 0;
        }

        public async Task<Cliente> ObterPorEmail(string email, CancellationToken cancellationToken)
        {
            return await _clienteCollection.Find(x => x.Email.ToLower() == email.ToLower() && x.Excluido == null).FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<Cliente> ObterPorEmailCadastroAtivo(string email, CancellationToken cancellationToken)
        {
            return await _clienteCollection.Find(x => x.Email.ToLower() == email.ToLower() && x.Excluido == null && x.Status).FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<Cliente> ObterPorHashAtivacaoCadastro(string hash, CancellationToken cancellationToken)
        {
            return await _clienteCollection.Find(x => x.HashAtivacaoCadastro == hash && x.DataAtivacaoCadastro == null && x.DataValidadeAtivacaoCadastro.Value >= DateTime.UtcNow && x.Excluido == null).FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<Cliente> ObterPorHashEsqueciSenha(string hash, CancellationToken cancellationToken)
        {
            return await _clienteCollection.Find(x => x.HashEsqueciSenha == hash && x.DataValidadeEsqueciSenha.Value >= DateTime.UtcNow && x.Excluido == null).FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<Cliente> ObterPorId(Guid id, CancellationToken cancellationToken)
        {
            return await _clienteCollection.Find(x => x.Id == id && x.Excluido == null).FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<IEnumerable<Cliente>> ObterTodos(string filtro, int currentPage, int pageSize, CancellationToken cancellationToken)
        {
            currentPage--;
            if (currentPage < 0)
                currentPage = 0;
            return await _clienteCollection.Find(x => x.Excluido == null && x.Nome.ToLower().Contains(filtro.ToLower())).Skip(currentPage * pageSize).Limit(pageSize).ToListAsync(cancellationToken);
        }
        public async Task<long> Count(CancellationToken cancellationToken)
        {
            var filter = new FilterDefinitionBuilder<Cliente>().Where(x => x.Excluido == null);
            return await _clienteCollection.CountDocumentsAsync(filter, null, cancellationToken);
        }
    }
}
