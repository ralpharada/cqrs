using Tellus.Domain.Interfaces;
using Tellus.Domain.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace Tellus.Infra.Repositories
{
    public class DocumentoRepository : IDocumentoRepository
    {
        private readonly IMongoCollection<Documento> documentoCollection;
        public DocumentoRepository(IOptions<DbSettings> _dbSettings)
        {
            var mongoClient = new MongoClient(_dbSettings.Value.ConnectionString);
            var mongoDataBase = mongoClient.GetDatabase(_dbSettings.Value.DatabaseName);
            documentoCollection = mongoDataBase.GetCollection<Documento>(nameof(Documento));
        }
        public async Task<ReplaceOneResult> Salvar(Documento entity, CancellationToken cancellationToken)
        {
            var filter = new FilterDefinitionBuilder<Documento>().Where(x => x.Id == entity.Id && x.ClienteId == entity.ClienteId);
            return await documentoCollection.ReplaceOneAsync(filter, entity, new ReplaceOptions { IsUpsert = true }, cancellationToken);
        }
        public async Task<bool> DeletarPorId(Guid id, Guid clienteId, CancellationToken cancellationToken)
        {
            var documento = await ObterPorId(id, clienteId, cancellationToken);
            documento.DataExclusao = DateTime.UtcNow;
            var filter = new FilterDefinitionBuilder<Documento>().Where(x => x.Id == id && x.ClienteId == clienteId);
            var result = await documentoCollection.ReplaceOneAsync(filter, documento, new ReplaceOptions { IsUpsert = true }, cancellationToken);
            return result.ModifiedCount > 0;
        }
        public async Task<bool> DeletarPorIds(List<Guid> ids, Guid clienteId, CancellationToken cancellationToken)
        {
            var filter = new FilterDefinitionBuilder<Documento>().Where(x => ids.Contains(x.Id) && x.ClienteId == clienteId);
            var result = await documentoCollection.DeleteManyAsync(filter, cancellationToken);
            return result.DeletedCount > 0;
        }
        public async Task<Documento> ObterPorId(Guid id, Guid clienteId, CancellationToken cancellationToken)
        {
            return await documentoCollection.Find(x => x.Id == id && x.ClienteId == clienteId).FirstOrDefaultAsync(cancellationToken);
        }
        public async Task<List<Documento>> ObterPorIds(List<Guid> ids, Guid clienteId, CancellationToken cancellationToken)
        {
            return await documentoCollection.Find(x => ids.Contains(x.Id) && x.ClienteId == clienteId).ToListAsync(cancellationToken);
        }
        public async Task<List<Documento>> Pesquisar(Guid clienteId, Guid tipoDocumentoId, string dataImportacaoDe, string dataImportacaoAte, List<IndiceValor> indiceValores, int currentPage, int pageSize, CancellationToken cancellationToken)
        {
            currentPage--;
            if (currentPage < 0)
                currentPage = 0;
            var filter = Builders<Documento>.Filter.Eq(x => x.ClienteId, clienteId);
            filter &= Builders<Documento>.Filter.Eq(x => x.TipoDocumentoId, tipoDocumentoId);
            filter &= Builders<Documento>.Filter.Where(x => x.DataExclusao == null);

            if (!String.IsNullOrEmpty(dataImportacaoDe))
                filter &= Builders<Documento>.Filter.Gte(x => x.DataCadastro, Convert.ToDateTime(dataImportacaoDe + " 00:00:00"));

            if (!String.IsNullOrEmpty(dataImportacaoAte))
                filter &= Builders<Documento>.Filter.Lte(x => x.DataCadastro, Convert.ToDateTime(dataImportacaoAte + " 23:59:59"));

            foreach (var indiceValor in indiceValores)
            {
                filter &= indiceValor.ETipoIndice switch
                {
                    "Data" => indiceValor.Operador switch
                    {
                        ">" => Builders<Documento>.Filter.Where(a => a.IndiceValores.Any(iv => iv.IndiceId == indiceValor.IndiceId && iv.Data > indiceValor.Data)),
                        ">=" => Builders<Documento>.Filter.Where(a => a.IndiceValores.Any(iv => iv.IndiceId == indiceValor.IndiceId && iv.Data >= indiceValor.Data)),
                        "<" => Builders<Documento>.Filter.Where(a => a.IndiceValores.Any(iv => iv.IndiceId == indiceValor.IndiceId && iv.Data < indiceValor.Data)),
                        "<=" => Builders<Documento>.Filter.Where(a => a.IndiceValores.Any(iv => iv.IndiceId == indiceValor.IndiceId && iv.Data <= indiceValor.Data)),
                        _ => Builders<Documento>.Filter.Where(a => a.IndiceValores.Any(iv => iv.IndiceId == indiceValor.IndiceId && iv.Data == indiceValor.Data)),
                    },
                    "Hora" => indiceValor.Operador switch
                    {
                        ">" => Builders<Documento>.Filter.Where(a => a.IndiceValores.Any(iv => iv.IndiceId == indiceValor.IndiceId && iv.Hora > indiceValor.Hora)),
                        ">=" => Builders<Documento>.Filter.Where(a => a.IndiceValores.Any(iv => iv.IndiceId == indiceValor.IndiceId && iv.Hora >= indiceValor.Hora)),
                        "<" => Builders<Documento>.Filter.Where(a => a.IndiceValores.Any(iv => iv.IndiceId == indiceValor.IndiceId && iv.Hora < indiceValor.Hora)),
                        "<=" => Builders<Documento>.Filter.Where(a => a.IndiceValores.Any(iv => iv.IndiceId == indiceValor.IndiceId && iv.Hora <= indiceValor.Hora)),
                        _ => Builders<Documento>.Filter.Where(a => a.IndiceValores.Any(iv => iv.IndiceId == indiceValor.IndiceId && iv.Hora == indiceValor.Hora)),
                    },
                    "Numero" => indiceValor.Operador switch
                    {
                        ">" => Builders<Documento>.Filter.Where(a => a.IndiceValores.Any(iv => iv.IndiceId == indiceValor.IndiceId && iv.Numero > indiceValor.Numero)),
                        ">=" => Builders<Documento>.Filter.Where(a => a.IndiceValores.Any(iv => iv.IndiceId == indiceValor.IndiceId && iv.Numero >= indiceValor.Numero)),
                        "<" => Builders<Documento>.Filter.Where(a => a.IndiceValores.Any(iv => iv.IndiceId == indiceValor.IndiceId && iv.Numero < indiceValor.Numero)),
                        "<=" => Builders<Documento>.Filter.Where(a => a.IndiceValores.Any(iv => iv.IndiceId == indiceValor.IndiceId && iv.Numero <= indiceValor.Numero)),
                        _ => Builders<Documento>.Filter.Where(a => a.IndiceValores.Any(iv => iv.IndiceId == indiceValor.IndiceId && iv.Numero == indiceValor.Numero)),
                    },
                    "Decimal" => indiceValor.Operador switch
                    {
                        ">" => Builders<Documento>.Filter.Where(a => a.IndiceValores.Any(iv => iv.IndiceId == indiceValor.IndiceId && iv.Decimal > indiceValor.Decimal)),
                        ">=" => Builders<Documento>.Filter.Where(a => a.IndiceValores.Any(iv => iv.IndiceId == indiceValor.IndiceId && iv.Decimal >= indiceValor.Decimal)),
                        "<" => Builders<Documento>.Filter.Where(a => a.IndiceValores.Any(iv => iv.IndiceId == indiceValor.IndiceId && iv.Decimal < indiceValor.Decimal)),
                        "<=" => Builders<Documento>.Filter.Where(a => a.IndiceValores.Any(iv => iv.IndiceId == indiceValor.IndiceId && iv.Decimal <= indiceValor.Decimal)),
                        _ => Builders<Documento>.Filter.Where(a => a.IndiceValores.Any(iv => iv.IndiceId == indiceValor.IndiceId && iv.Decimal == indiceValor.Decimal)),
                    },
                    _ => indiceValor.Operador switch
                    {
                        "A*" => Builders<Documento>.Filter.Where(a => a.IndiceValores.Any(iv => iv.IndiceId == indiceValor.IndiceId && iv.Texto.ToLower().StartsWith(indiceValor.Texto.ToLower()))),
                        "*A" => Builders<Documento>.Filter.Where(a => a.IndiceValores.Any(iv => iv.IndiceId == indiceValor.IndiceId && iv.Texto.ToLower().EndsWith(indiceValor.Texto.ToLower()))),
                        "*A*" => Builders<Documento>.Filter.Where(a => a.IndiceValores.Any(iv => iv.IndiceId == indiceValor.IndiceId && iv.Texto.ToLower().Contains(indiceValor.Texto.ToLower()))),
                        "< >" => Builders<Documento>.Filter.Where(a => a.IndiceValores.Any(iv => iv.IndiceId == indiceValor.IndiceId && iv.Texto != indiceValor.Texto)),
                        _ => Builders<Documento>.Filter.Where(a => a.IndiceValores.Any(iv => iv.IndiceId == indiceValor.IndiceId && iv.Texto.ToLower() == indiceValor.Texto.ToLower())),
                    },
                };
            }
            return await documentoCollection.Find(filter).Skip(currentPage * pageSize).Limit(pageSize).ToListAsync(cancellationToken);

        }
        public async Task<long> Count(Guid clienteId, Guid tipoDocumentoId, string dataImportacaoDe, string dataImportacaoAte, List<IndiceValor> indiceValores, CancellationToken cancellationToken)
        {
            var filter = Builders<Documento>.Filter.Eq(x => x.ClienteId, clienteId);
            filter &= Builders<Documento>.Filter.Eq(x => x.TipoDocumentoId, tipoDocumentoId);
            filter &= Builders<Documento>.Filter.Where(x => x.DataExclusao == null);

            if (!String.IsNullOrEmpty(dataImportacaoDe))
                filter &= Builders<Documento>.Filter.Gte(x => x.DataCadastro, Convert.ToDateTime(dataImportacaoDe + " 00:00:00"));

            if (!String.IsNullOrEmpty(dataImportacaoAte))
                filter &= Builders<Documento>.Filter.Lte(x => x.DataCadastro, Convert.ToDateTime(dataImportacaoAte + " 23:59:59"));

            foreach (var indiceValor in indiceValores)
            {
                filter &= indiceValor.ETipoIndice switch
                {
                    "Data" => indiceValor.Operador switch
                    {
                        ">" => Builders<Documento>.Filter.Where(a => a.IndiceValores.Any(iv => iv.IndiceId == indiceValor.IndiceId && iv.Data > indiceValor.Data)),
                        ">=" => Builders<Documento>.Filter.Where(a => a.IndiceValores.Any(iv => iv.IndiceId == indiceValor.IndiceId && iv.Data >= indiceValor.Data)),
                        "<" => Builders<Documento>.Filter.Where(a => a.IndiceValores.Any(iv => iv.IndiceId == indiceValor.IndiceId && iv.Data < indiceValor.Data)),
                        "<=" => Builders<Documento>.Filter.Where(a => a.IndiceValores.Any(iv => iv.IndiceId == indiceValor.IndiceId && iv.Data <= indiceValor.Data)),
                        _ => Builders<Documento>.Filter.Where(a => a.IndiceValores.Any(iv => iv.IndiceId == indiceValor.IndiceId && iv.Data == indiceValor.Data)),
                    },
                    "Hora" => indiceValor.Operador switch
                    {
                        ">" => Builders<Documento>.Filter.Where(a => a.IndiceValores.Any(iv => iv.IndiceId == indiceValor.IndiceId && iv.Hora > indiceValor.Hora)),
                        ">=" => Builders<Documento>.Filter.Where(a => a.IndiceValores.Any(iv => iv.IndiceId == indiceValor.IndiceId && iv.Hora >= indiceValor.Hora)),
                        "<" => Builders<Documento>.Filter.Where(a => a.IndiceValores.Any(iv => iv.IndiceId == indiceValor.IndiceId && iv.Hora < indiceValor.Hora)),
                        "<=" => Builders<Documento>.Filter.Where(a => a.IndiceValores.Any(iv => iv.IndiceId == indiceValor.IndiceId && iv.Hora <= indiceValor.Hora)),
                        _ => Builders<Documento>.Filter.Where(a => a.IndiceValores.Any(iv => iv.IndiceId == indiceValor.IndiceId && iv.Hora == indiceValor.Hora)),
                    },
                    "Numero" => indiceValor.Operador switch
                    {
                        ">" => Builders<Documento>.Filter.Where(a => a.IndiceValores.Any(iv => iv.IndiceId == indiceValor.IndiceId && iv.Numero > indiceValor.Numero)),
                        ">=" => Builders<Documento>.Filter.Where(a => a.IndiceValores.Any(iv => iv.IndiceId == indiceValor.IndiceId && iv.Numero >= indiceValor.Numero)),
                        "<" => Builders<Documento>.Filter.Where(a => a.IndiceValores.Any(iv => iv.IndiceId == indiceValor.IndiceId && iv.Numero < indiceValor.Numero)),
                        "<=" => Builders<Documento>.Filter.Where(a => a.IndiceValores.Any(iv => iv.IndiceId == indiceValor.IndiceId && iv.Numero <= indiceValor.Numero)),
                        _ => Builders<Documento>.Filter.Where(a => a.IndiceValores.Any(iv => iv.IndiceId == indiceValor.IndiceId && iv.Numero == indiceValor.Numero)),
                    },
                    "Decimal" => indiceValor.Operador switch
                    {
                        ">" => Builders<Documento>.Filter.Where(a => a.IndiceValores.Any(iv => iv.IndiceId == indiceValor.IndiceId && iv.Decimal > indiceValor.Decimal)),
                        ">=" => Builders<Documento>.Filter.Where(a => a.IndiceValores.Any(iv => iv.IndiceId == indiceValor.IndiceId && iv.Decimal >= indiceValor.Decimal)),
                        "<" => Builders<Documento>.Filter.Where(a => a.IndiceValores.Any(iv => iv.IndiceId == indiceValor.IndiceId && iv.Decimal < indiceValor.Decimal)),
                        "<=" => Builders<Documento>.Filter.Where(a => a.IndiceValores.Any(iv => iv.IndiceId == indiceValor.IndiceId && iv.Decimal <= indiceValor.Decimal)),
                        _ => Builders<Documento>.Filter.Where(a => a.IndiceValores.Any(iv => iv.IndiceId == indiceValor.IndiceId && iv.Decimal == indiceValor.Decimal)),
                    },
                    _ => indiceValor.Operador switch
                    {
                        "A*" => Builders<Documento>.Filter.Where(a => a.IndiceValores.Any(iv => iv.IndiceId == indiceValor.IndiceId && iv.Texto.ToLower().StartsWith(indiceValor.Texto.ToLower()))),
                        "*A" => Builders<Documento>.Filter.Where(a => a.IndiceValores.Any(iv => iv.IndiceId == indiceValor.IndiceId && iv.Texto.ToLower().EndsWith(indiceValor.Texto.ToLower()))),
                        "*A*" => Builders<Documento>.Filter.Where(a => a.IndiceValores.Any(iv => iv.IndiceId == indiceValor.IndiceId && iv.Texto.ToLower().Contains(indiceValor.Texto.ToLower()))),
                        "< >" => Builders<Documento>.Filter.Where(a => a.IndiceValores.Any(iv => iv.IndiceId == indiceValor.IndiceId && iv.Texto != indiceValor.Texto)),
                        _ => Builders<Documento>.Filter.Where(a => a.IndiceValores.Any(iv => iv.IndiceId == indiceValor.IndiceId && iv.Texto.ToLower() == indiceValor.Texto.ToLower())),
                    },
                };
            }
            return await documentoCollection.CountDocumentsAsync(filter, null, cancellationToken);

        }
        public async Task<List<Documento>> ObterTotalArmazenamento(Guid clienteId, CancellationToken cancellationToken)
        {
            return await documentoCollection.Find(x => x.ClienteId == clienteId).ToListAsync(cancellationToken);
        }
        public async Task<List<Guid>> PesquisaRapidaTiposDocumento(Guid clienteId, string pesquisa)
        {

            var result = documentoCollection
    .AsQueryable()
    .AsEnumerable()
    .Where(x => x.ClienteId == clienteId && x.DataExclusao == null && x.IndiceValores.Any(iv => iv.Texto != null && iv.Texto.ToLower().Contains(pesquisa.ToLower())))
    .GroupBy(x => x.TipoDocumentoId)
    .Select(grp => grp.Key)
    .ToList();
            return result;

        }
        public async Task<List<Documento>> PesquisaRapida(Guid clienteId, Guid tipoDocumentoId, string pesquisa, int currentPage, int pageSize, CancellationToken cancellationToken)
        {
            currentPage--;
            if (currentPage < 0)
                currentPage = 0;
            var filter = Builders<Documento>.Filter.Eq(x => x.ClienteId, clienteId);
            filter &= Builders<Documento>.Filter.Eq(x => x.TipoDocumentoId, tipoDocumentoId);
            filter &= Builders<Documento>.Filter.Where(a => a.IndiceValores.Any(iv => iv.Texto.ToLower().Contains(pesquisa.ToLower())));
            filter &= Builders<Documento>.Filter.Where(x => x.DataExclusao == null);
            return await documentoCollection.Find(filter).SortBy(x => x.TipoDocumentoId).Skip(currentPage * pageSize).Limit(pageSize).ToListAsync(cancellationToken);

        }
        public async Task<long> CountPesquisaRapida(Guid clienteId, Guid tipoDocumentoId, string pesquisa, CancellationToken cancellationToken)
        {
            var filter = Builders<Documento>.Filter.Eq(x => x.ClienteId, clienteId);
            filter &= Builders<Documento>.Filter.Eq(x => x.TipoDocumentoId, tipoDocumentoId);
            filter &= Builders<Documento>.Filter.Where(a => a.IndiceValores.Any(iv => iv.Texto.ToLower().Contains(pesquisa.ToLower())));
            filter &= Builders<Documento>.Filter.Where(x => x.DataExclusao == null);
            return await documentoCollection.CountDocumentsAsync(filter, null, cancellationToken);

        }
        public async Task<List<Documento>> PesquisaDocumentosExcluidos(Guid clienteId, int currentPage, int pageSize, CancellationToken cancellationToken)
        {
            currentPage--;
            if (currentPage < 0)
                currentPage = 0;
            return await documentoCollection.Find(x => x.ClienteId == clienteId && x.DataExclusao != null).SortBy(x => x.TipoDocumentoId).Skip(currentPage * pageSize).Limit(pageSize).ToListAsync(cancellationToken);

        }
        public async Task<long> CountPesquisaDocumentosExcluidos(Guid clienteId, CancellationToken cancellationToken)
        {
            var filter = Builders<Documento>.Filter.Eq(x => x.ClienteId, clienteId);
            return await documentoCollection.Find(x => x.ClienteId == clienteId && x.DataExclusao != null).CountAsync(cancellationToken);

        }
        public async Task<long> CountTotalDocumentos(Guid clienteId, CancellationToken cancellationToken)
        {
            var filter = Builders<Documento>.Filter.Eq(x => x.ClienteId, clienteId);
            return await documentoCollection.CountAsync(filter, null, cancellationToken);
        }
        public async Task<long> CountTotalDocumentosPorUsuario(Guid usuarioId, CancellationToken cancellationToken)
        {
            var filter = Builders<Documento>.Filter.Eq(x => x.UsuarioId, usuarioId);
            return await documentoCollection.CountAsync(filter, null, cancellationToken);
        }
        public async Task<List<Documento>> ObterPorCliente(Guid clienteId, CancellationToken cancellationToken)
        {
            return await documentoCollection.Find(x => x.ClienteId == clienteId).ToListAsync(cancellationToken);
        }
        public async Task<List<Documento>> DocumentoPorClienteDataDe(Guid clienteId, string dataImportacaoAte, CancellationToken cancellationToken)
        {
            var filter = Builders<Documento>.Filter.Eq(x => x.ClienteId, clienteId);

            if (!String.IsNullOrEmpty(dataImportacaoAte))
                filter &= Builders<Documento>.Filter.Gte(x => x.DataCadastro, Convert.ToDateTime(dataImportacaoAte));

            return await documentoCollection.Find(filter).ToListAsync(cancellationToken);

        }
        public async Task<bool> ExisteDocumentoAssociadoTipoDocumento(Guid tipoDOcumentoId, Guid clienteId, CancellationToken cancellationToken)
        {
            var documento = await documentoCollection.Find(x => x.ClienteId == clienteId && x.TipoDocumentoId == tipoDOcumentoId).FirstOrDefaultAsync(cancellationToken);
            return documento != null;
        }
        public async Task<List<Documento>> ObterPorUsuario(Guid usuarioId, CancellationToken cancellationToken)
        {
            return await documentoCollection.Find(x => x.UsuarioId == usuarioId).ToListAsync(cancellationToken);
        }
        public async Task<List<Documento>> DocumentoPorUsuarioDataDe(Guid usuarioId, string dataImportacaoAte, CancellationToken cancellationToken)
        {
            var filter = Builders<Documento>.Filter.Eq(x => x.UsuarioId, usuarioId);

            if (!String.IsNullOrEmpty(dataImportacaoAte))
                filter &= Builders<Documento>.Filter.Gte(x => x.DataCadastro, Convert.ToDateTime(dataImportacaoAte));

            return await documentoCollection.Find(filter).ToListAsync(cancellationToken);

        }
    }
}

