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
    public class ObterDocumentoPorIdHandler : IRequestHandler<ObterDocumentoPorIdQuery, IEvent>
    {
        private readonly IDocumentoRepository _repository;
        private readonly ITipoDocumentoRepository _repositoryTipoDocumento;
        private readonly IIndiceRepository _repositoryIndice;
        private readonly IUsuarioRepository _repositoryUsuario;
        private readonly UsuarioAutenticado _usuarioAutenticado;
        private readonly IClienteRepository _repositoryCliente;
        private readonly IAmazonRepository _repositoryAmazon;
        private readonly IConfiguration _configuration;

        public ObterDocumentoPorIdHandler(IDocumentoRepository repository,
            ITipoDocumentoRepository repositoryTipoDocumento,
            IIndiceRepository repositoryIndice,
            UsuarioAutenticado usuarioAutenticado,
            IUsuarioRepository repositoryUsuario,
            IClienteRepository repositoryCliente,
            IAmazonRepository repositoryAmazon,
            IConfiguration configuration)
        {
            _repository = repository;
            _repositoryTipoDocumento = repositoryTipoDocumento;
            _repositoryIndice = repositoryIndice;
            _repositoryUsuario = repositoryUsuario;
            _usuarioAutenticado = usuarioAutenticado;
            _repositoryCliente = repositoryCliente;
            _repositoryAmazon = repositoryAmazon;
            _configuration = configuration;
        }
        public async Task<IEvent> Handle(ObterDocumentoPorIdQuery request, CancellationToken cancellationToken)
        {
            bool success = false;
            var response = new DocumentoResponse();
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
                        var documento = await _repository.ObterPorId(request.Id, usuario.ClienteId, cancellationToken);
                        var tipoDocumento = await _repositoryTipoDocumento.ObterPorId(documento.TipoDocumentoId, usuario.ClienteId, cancellationToken);
                        var tipoDocumentoResponse = TipoDocumentoMapper<TipoDocumentoResponse>.Map(tipoDocumento);
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
                        var file = await _repositoryAmazon.Download(_configuration.GetSection("AmazonS2:ACCESS_KEY_ID").Value, _configuration.GetSection("AmazonS2:SECRET_ACCESS_KEY").Value, _configuration.GetSection("AmazonS2:Bucket").Value, cliente.Id.ToString(), documento.Id.ToString());
                        response = new()
                        {
                            Id = documento.Id,
                            TipoDocumento = tipoDocumentoResponse,
                            IndiceValores = indiceValorResponses,
                            NomeArquivo = documento.NomeArquivo,
                            TamanhoArquivo = documento.TamanhoArquivo,
                            Arquivo = file,
                            FormatoArquivo = documento.FormatoArquivo,
                            DataCadastro = documento.DataCadastro,
                            DataUltimaAlteracao = documento.DataUltimaAlteracao
                        };
                        success = true;
                    }
                }
            }
            catch (Exception ex)
            {
                return new ResultEvent(success, ex.Message);
            }
            return new ResultEvent(success, response);
        }

    }
}
