using Tellus.Application.Core;
using Tellus.Application.Queries;
using Tellus.Application.Responses;
using Tellus.Core.Events;
using Tellus.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Configuration;

namespace Tellus.Application.Handlers
{
    public class ObterDocumentosPorIdsHandler : IRequestHandler<ObterDocumentosPorIdsQuery, IEvent>
    {
        private readonly IDocumentoRepository _repository;
        private readonly IUsuarioRepository _repositoryUsuario;
        private readonly UsuarioAutenticado _usuarioAutenticado;
        private readonly IClienteRepository _repositoryCliente;
        private readonly IAmazonRepository _repositoryAmazon;
        private readonly IConfiguration _configuration;

        public ObterDocumentosPorIdsHandler(IDocumentoRepository repository, UsuarioAutenticado usuarioAutenticado, IUsuarioRepository repositoryUsuario, IClienteRepository repositoryCliente, IAmazonRepository repositoryAmazon, IConfiguration configuration)
        {
            _repository = repository;
            _repositoryUsuario = repositoryUsuario;
            _usuarioAutenticado = usuarioAutenticado;
            _repositoryCliente = repositoryCliente;
            _repositoryAmazon = repositoryAmazon;
            _configuration = configuration;
        }
        public async Task<IEvent> Handle(ObterDocumentosPorIdsQuery request, CancellationToken cancellationToken)
        {
            bool success = false;
            var lista = new List<DocumentoResponse>();
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
                        var documentos = await _repository.ObterPorIds(request.Ids, usuario.ClienteId, cancellationToken);
                        foreach (var documento in documentos)
                        {
                            var file = await _repositoryAmazon.Download(_configuration.GetSection("AmazonS2:ACCESS_KEY_ID").Value, _configuration.GetSection("AmazonS2:SECRET_ACCESS_KEY").Value, _configuration.GetSection("AmazonS2:Bucket").Value, cliente.Id.ToString(), documento.Id.ToString());
                            DocumentoResponse documentoResponse = new()
                            {
                                Id = documento.Id,
                                NomeArquivo = documento.NomeArquivo,
                                Arquivo = string.Concat(documento.FormatoArquivo, ",", file),
                            };

                            lista.Add(documentoResponse);
                        }
                        success = true;
                    }
                }
            }
            catch (Exception ex)
            {
                return new ResultEvent(success, ex.Message);
            }
            return new ResultEvent(success, lista);
        }

    }
}
