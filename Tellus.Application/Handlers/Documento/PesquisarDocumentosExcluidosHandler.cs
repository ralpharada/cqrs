using Tellus.Application.Core;
using Tellus.Application.Mapper;
using Tellus.Application.Queries;
using Tellus.Application.Responses;
using Tellus.Core.Events;
using Tellus.Domain.Interfaces;
using Tellus.Domain.Models;
using MediatR;
using Microsoft.Extensions.Configuration;

namespace Tellus.Application.Handlers
{
    public class PesquisarDocumentosExcluidosHandler : IRequestHandler<PesquisarDocumentosExcluidosQuery, IEvent>
    {
        private readonly IDocumentoRepository _repository;
        private readonly ITipoDocumentoRepository _repositoryTipoDocumento;
        private readonly IIndiceRepository _repositoryIndice;
        private readonly IConfiguration _configuration;
        private readonly IUsuarioRepository _repositoryUsuario;
        private readonly UsuarioAutenticado _usuarioAutenticado;
        private readonly IClienteRepository _repositoryCliente;
        private readonly IAmazonRepository _repositoryAmazon;

        public PesquisarDocumentosExcluidosHandler(IDocumentoRepository repository, ITipoDocumentoRepository repositoryTipoDocumento, IIndiceRepository repositoryIndice, IConfiguration configuration, UsuarioAutenticado usuarioAutenticado, IUsuarioRepository repositoryUsuario, IClienteRepository repositoryCliente, IAmazonRepository repositoryAmazon)
        {
            _repository = repository;
            _repositoryTipoDocumento = repositoryTipoDocumento;
            _repositoryIndice = repositoryIndice;
            _configuration = configuration;
            _repositoryUsuario = repositoryUsuario;
            _usuarioAutenticado = usuarioAutenticado;
            _repositoryCliente = repositoryCliente;
            _repositoryAmazon = repositoryAmazon;
        }
        public async Task<IEvent> Handle(PesquisarDocumentosExcluidosQuery request, CancellationToken cancellationToken)
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
                        var result = await _repository.PesquisaDocumentosExcluidos(usuario.ClienteId, request.CurrentPage, Convert.ToInt32(_configuration.GetSection("PageSize").Value), cancellationToken);
                        total = await _repository.CountPesquisaDocumentosExcluidos(usuario.ClienteId, cancellationToken);
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
                                    Valor = indiceValor.Texto
                                });
                            }
                            DocumentoResponse documentoResponse = new()
                            {
                                Id = documento.Id,
                                IndiceValores = indiceValorResponses,
                                NomeArquivo = documento.NomeArquivo,
                                DataCadastro = documento.DataCadastro,
                                DataUltimaAlteracao = documento.DataUltimaAlteracao,
                                DataExclusao = documento.DataExclusao.Value
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
