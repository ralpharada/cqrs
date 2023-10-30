using Tellus.Application.Core;
using Tellus.Application.Mapper;
using Tellus.Application.Queries;
using Tellus.Application.Responses;
using Tellus.Core.Events;
using Tellus.Domain.Interfaces;
using Tellus.Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace Tellus.Application.Handlers
{
    public class PesquisarDocumentoHandler : IRequestHandler<PesquisarDocumentoQuery, IEvent>
    {
        private readonly IDocumentoRepository _repository;
        private readonly ITipoDocumentoRepository _repositoryTipoDocumento;
        private readonly IIndiceRepository _repositoryIndice;
        private readonly IConfiguration _configuration;
        private readonly IUsuarioRepository _repositoryUsuario;
        private readonly UsuarioAutenticado _usuarioAutenticado;
        private readonly IClienteRepository _repositoryCliente;
        private readonly IAmazonRepository _repositoryAmazon;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMediator _mediator;

        public PesquisarDocumentoHandler(IDocumentoRepository repository, ITipoDocumentoRepository repositoryTipoDocumento, IIndiceRepository repositoryIndice, IConfiguration configuration, UsuarioAutenticado usuarioAutenticado, IUsuarioRepository repositoryUsuario, IClienteRepository repositoryCliente, IAmazonRepository repositoryAmazon,
        IHttpContextAccessor httpContextAccessor,
        IMediator mediator)
        {
            _repository = repository;
            _repositoryTipoDocumento = repositoryTipoDocumento;
            _repositoryIndice = repositoryIndice;
            _configuration = configuration;
            _repositoryUsuario = repositoryUsuario;
            _usuarioAutenticado = usuarioAutenticado;
            _repositoryCliente = repositoryCliente;
            _repositoryAmazon = repositoryAmazon;
            _httpContextAccessor = httpContextAccessor;
            _mediator = mediator;
        }
        public async Task<IEvent> Handle(PesquisarDocumentoQuery request, CancellationToken cancellationToken)
        {
            string mensagem = "";
            bool success = false;
            Guid id = Guid.Empty;
            var lista = new List<DocumentoResponse>();
            long total = 0;
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
                        var ip = _httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();
                        await _mediator.Send(new AtualizarUsuarioLogadoQuery(usuario.Id, ip), cancellationToken);

                        var tipoDocumento = await _repositoryTipoDocumento.ObterPorId(request.TipoDocumentoId, usuario.ClienteId, cancellationToken);
                        var tipoDocumentoResponse = TipoDocumentoMapper<TipoDocumentoResponse>.Map(tipoDocumento);
                        var result = await _repository.Pesquisar(usuario.ClienteId, request.TipoDocumentoId, request.DataImportacaoDe, request.DataImportacaoAte, request.IndiceValores, request.CurrentPage, Convert.ToInt32(_configuration.GetSection("PageSize").Value), cancellationToken);
                        total = await _repository.Count(usuario.ClienteId, request.TipoDocumentoId, request.DataImportacaoDe, request.DataImportacaoAte, request.IndiceValores, cancellationToken);
                        foreach (var documento in result)
                        {
                            List<IndiceValorResponse> indiceValorResponses = new List<IndiceValorResponse>();
                            List<Guid> idIndices = new List<Guid>();
                            documento.IndiceValores.ForEach(i => idIndices.Add(i.IndiceId));
                            List<Indice> indices = _repositoryIndice.ObterTodosPorIds(idIndices, usuario.ClienteId);
                            foreach (var indiceValor in documento.IndiceValores)
                            {
                                indiceValorResponses.Add(new()
                                {
                                    Indice = IndiceMapper<IndiceResponse>.Map(indices.Find(x => x.Id == indiceValor.IndiceId)),
                                    Valor = indiceValor.ETipoIndice switch
                                    {
                                        "Data" => indiceValor.Data.HasValue ? indiceValor.Data.Value.ToShortDateString() : null,
                                        "Hora" => indiceValor.Hora.HasValue ? indiceValor.Hora.Value.ToShortTimeString() : null,
                                        "Decimal" => indiceValor.Decimal.HasValue ? indiceValor.Decimal.Value.ToString() : null,
                                        "Numero" => indiceValor.Numero.HasValue ? indiceValor.Numero.Value.ToString() : null,
                                        _ => indiceValor.Texto
                                    }
                                });
                            }
                      //      var file = await _repositoryAmazon.Download(_configuration.GetSection("AmazonS2:ACCESS_KEY_ID").Value, _configuration.GetSection("AmazonS2:SECRET_ACCESS_KEY").Value, _configuration.GetSection("AmazonS2:Bucket").Value, documento.NomeArquivo);
                            DocumentoResponse documentoResponse = new()
                            {
                                Id = documento.Id,
                                TipoDocumento = tipoDocumentoResponse,
                                IndiceValores = indiceValorResponses,
                                NomeArquivo = documento.NomeArquivo,
                             //  Arquivo = file,
                                DataCadastro = documento.DataCadastro,
                                DataUltimaAlteracao = documento.DataUltimaAlteracao
                            };
                            lista.Add(documentoResponse);
                        }
                        success = true;
                    }
                }
            }
            catch (Exception ex)
            {
                mensagem = "Falha ao tentar efetuar o cadastro.";
                return new ResultEvent(success, mensagem);
            }
            return new ResultEvent(success, lista, total);
        }

    }
}
