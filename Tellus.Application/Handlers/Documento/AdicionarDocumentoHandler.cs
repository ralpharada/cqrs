using Tellus.Application.Core;
using Tellus.Application.Queries;
using Tellus.Core.Events;
using Tellus.Domain.Enums;
using Tellus.Domain.Interfaces;
using Tellus.Domain.Models;
using MediatR;
using Microsoft.Extensions.Configuration;

namespace Tellus.Application.Handlers
{
    public class AdicionarDocumentoHandler : IRequestHandler<AdicionarDocumentoQuery, IEvent>
    {
        private readonly IDocumentoRepository _repository;
        private readonly IIndiceRepository _repositoryIndice;
        private readonly IAmazonRepository _repositoryAmazon;
        private readonly ITipoDocumentoRepository _repositoryTipoDocumento;
        private readonly ILogDocumentoRepository _repositoryLogDocumento;
        private readonly IUsuarioRepository _repositoryUsuario;
        private readonly IClienteRepository _repositoryCliente;
        private readonly UsuarioAutenticado _usuarioAutenticado;
        private readonly IConfiguration _configuration;

        public AdicionarDocumentoHandler(IDocumentoRepository repository,
            IAmazonRepository repositoryAmazon,
            IIndiceRepository repositoryIndice,
            ITipoDocumentoRepository repositoryTipoDocumento,
            ILogDocumentoRepository repositoryLogDocumento,
            UsuarioAutenticado usuarioAutenticado,
            IUsuarioRepository repositoryUsuario,
            IClienteRepository repositoryCliente,
            IConfiguration configuration)
        {
            _repository = repository;
            _repositoryIndice = repositoryIndice;
            _repositoryAmazon = repositoryAmazon;
            _repositoryTipoDocumento = repositoryTipoDocumento;
            _repositoryLogDocumento = repositoryLogDocumento;
            _repositoryUsuario = repositoryUsuario;
            _repositoryCliente = repositoryCliente;
            _usuarioAutenticado = usuarioAutenticado;
            _configuration = configuration;
        }
        public async Task<IEvent> Handle(AdicionarDocumentoQuery request, CancellationToken cancellationToken)
        {
            Object mensagem = null;
            bool success = false;
            Guid id = Guid.Empty;
            try
            {
                if (_usuarioAutenticado.Email == null)
                    return new ResultEvent(success, "Acesso expirado");

                var usuario = await _repositoryUsuario.ObterPorEmailCadastroAtivo(_usuarioAutenticado.Email, cancellationToken);
                if (usuario != null)
                {
                    var cliente = await _repositoryCliente.ObterPorId(usuario.ClienteId, cancellationToken);
                    if (cliente != null && cliente.Status)
                    {
                        var documentos = await _repository.ObterTotalArmazenamento(usuario.ClienteId, cancellationToken);
                        var total = 0M;
                        if (documentos != null)
                        {
                            total += documentos.Sum(x => x.TamanhoArquivo);
                        }
                        double totalGB = cliente.EspacoDisco * 1024;
                        double megabytes = (double)(total + request.TamanhoArquivo) * 9.537e-7;
                        double megabytesFaltantes = totalGB - ((double)total * 9.537e-7);
                        if (totalGB >= megabytes)
                        {
                            var tipoDocumento = await _repositoryTipoDocumento.ObterPorId(request.TipoDocumentoId, usuario.ClienteId, cancellationToken);
                            if (tipoDocumento.IndiceIds != null)
                            {
                                var indices = _repositoryIndice.ObterTodosPorIds(tipoDocumento.IndiceIds, usuario.ClienteId);
                                foreach (var indice in indices)
                                {
                                    if (!request.IndiceValores.Exists(x => x.IndiceId == indice.Id))
                                    {
                                        IndiceValor indiceValor = new IndiceValor();
                                        indiceValor.IndiceId = indice.Id;
                                        indiceValor.ETipoIndice = indice.ETipoIndice.ToString();
                                        switch (indice.ETipoIndice)
                                        {
                                            case ETipoIndice.Numero:
                                                indiceValor.Numero = null;
                                                break;
                                            case ETipoIndice.Booleano:
                                                break;
                                            case ETipoIndice.Data:
                                                indiceValor.Data = null;
                                                break;
                                            case ETipoIndice.Hora:
                                                indiceValor.Hora = null;
                                                break;
                                            case ETipoIndice.Decimal:
                                                indiceValor.Decimal = null;
                                                break;
                                            case ETipoIndice.Lista:
                                                break;
                                            default:
                                                indiceValor.Texto = null;
                                                break;
                                        }
                                        request.IndiceValores.Add(indiceValor);
                                    }
                                }
                            }
                            request.IndiceValores.ForEach(x =>
                            {
                                switch (Enum.Parse(typeof(ETipoIndice), x.ETipoIndice))
                                {
                                    case ETipoIndice.Numero:
                                        x.Texto = x.Numero.ToString();
                                        break;
                                    case ETipoIndice.Booleano:
                                        break;
                                    case ETipoIndice.Data:
                                        x.Texto = x.Data.Value.ToShortDateString();
                                        break;
                                    case ETipoIndice.Hora:
                                        x.Texto = x.Hora.Value.ToShortTimeString();
                                        break;
                                    case ETipoIndice.Decimal:
                                        x.Texto = x.Decimal.ToString();
                                        break;
                                    case ETipoIndice.Lista:
                                        break;
                                    default:
                                        break;
                                }
                            });

                            string fileExtension = Path.GetExtension(request.NomeArquivo);

                            Documento documento = new()
                            {
                                Id = Guid.NewGuid(),
                                ClienteId = usuario.ClienteId,
                                TipoDocumentoId = request.TipoDocumentoId,
                                NomeArquivo = request.NomeArquivo,
                                TamanhoArquivo = request.TamanhoArquivo,
                                IndiceValores = request.IndiceValores,
                                UsuarioId = usuario.Id,
                                FormatoArquivo = request.FormatoArquivo,
                                DataCadastro = DateTime.UtcNow,
                                DataUltimaAlteracao = DateTime.UtcNow
                            };
                            var uploadOK = await _repositoryAmazon.SignedUrl(_configuration.GetSection("AmazonS2:ACCESS_KEY_ID").Value, _configuration.GetSection("AmazonS2:SECRET_ACCESS_KEY").Value, _configuration.GetSection("AmazonS2:Bucket").Value, cliente.Id.ToString(), documento.Id.ToString());
                            if (!String.IsNullOrEmpty(uploadOK))
                            {
                                var result = await _repository.Salvar(documento, cancellationToken);
                                success = result.UpsertedId > 0;
                                if (success)
                                {

                                    id = ((Guid)result.UpsertedId);
                                    mensagem = new { msg = "Documento cadastrado com sucesso!", signedUrl = uploadOK };
                                    LogDocumento logDocumento = new()
                                    {
                                        Id = Guid.NewGuid(),
                                        ClienteId = usuario.ClienteId,
                                        DocumentoId = documento.Id,
                                        DataRegistro = DateTime.UtcNow,
                                        Acao = "Inserido"
                                    };
                                    await _repositoryLogDocumento.Salvar(logDocumento, cancellationToken);
                                }
                            }
                        }
                        else
                        {
                            mensagem = "Tamanho do arquivo permitido: " + megabytesFaltantes.ToString("N2") + "MB";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                mensagem = "Falha ao tentar efetuar o cadastro.";
            }
            return new ResultEvent(success, mensagem, null, id);
        }

    }
}
