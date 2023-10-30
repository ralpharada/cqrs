using Tellus.Application.Core;
using Tellus.Application.Queries;
using Tellus.Application.Responses;
using Tellus.Core.Events;
using Tellus.Domain.Interfaces;
using Tellus.Domain.Models;
using MediatR;
using Microsoft.Extensions.Configuration;

namespace Tellus.Application.Handlers
{
    public class RelatorioDocumentosHandler : IRequestHandler<RelatorioDocumentosQuery, IEvent>
    {
        private readonly IClienteRepository _repositoryCliente;
        private readonly UsuarioAutenticado _usuarioAutenticado;
        private readonly IAmazonRepository _repositoryAmazon;
        private readonly IConfiguration _configuration;
        private readonly IDocumentoRepository _repositoryDocumento;
        private readonly IUsuarioRepository _repositoryUsuario;

        public RelatorioDocumentosHandler(IClienteRepository repositoryCliente,
            UsuarioAutenticado usuarioAutenticado,
            IAmazonRepository repositoryAmazon,
            IConfiguration configuration,
            IDocumentoRepository repositoryDocumento,
            IUsuarioRepository repositoryUsuario)
        {
            _repositoryCliente = repositoryCliente;
            _usuarioAutenticado = usuarioAutenticado;
            _repositoryAmazon = repositoryAmazon;
            _configuration = configuration;
            _repositoryDocumento = repositoryDocumento;
            _repositoryUsuario = repositoryUsuario;
        }

        public async Task<IEvent> Handle(RelatorioDocumentosQuery request, CancellationToken cancellationToken)
        {
            var success = false;
            var response = new List<RelatorioDocumentosResponse>();
            try
            {
                if (_usuarioAutenticado.Email == null)
                    return new ResultEvent(success, "Acesso expirado");

                var cliente = new Cliente();
                switch (request.Tipo)
                {
                    case "client":
                        cliente = await _repositoryCliente.ObterPorEmailCadastroAtivo(_usuarioAutenticado.Email, cancellationToken);
                        if (cliente != null)
                        {
                            var tamanhoPastaUtilizado = await _repositoryAmazon.GetFolderSizeInMB(_configuration.GetSection("AmazonS2:ACCESS_KEY_ID").Value, _configuration.GetSection("AmazonS2:SECRET_ACCESS_KEY").Value, _configuration.GetSection("AmazonS2:Bucket").Value, cliente.Id.ToString());
                            var totalDocumentos = await _repositoryDocumento.CountTotalDocumentos(cliente.Id, cancellationToken);
                            response.Add(new RelatorioDocumentosResponse()
                            {
                                Titulo = "Total de documentos armazenados",
                                Total = totalDocumentos
                            });
                            response.Add(new RelatorioDocumentosResponse()
                            {
                                Titulo = "GB ocupados",
                                Total = Math.Round(tamanhoPastaUtilizado / 1024, 4)
                            });
                            response.Add(new RelatorioDocumentosResponse()
                            {
                                Titulo = "GB disponíveis",
                                Total = Math.Round((((decimal)cliente.EspacoDisco * 1024) - tamanhoPastaUtilizado) / 1024, 4)
                            });
                            success = true;
                        }
                        break;
                    case "user":
                        var user = await _repositoryUsuario.ObterPorEmailCadastroAtivo(_usuarioAutenticado.Email, cancellationToken);
                        if (user != null)
                        {
                            cliente = await _repositoryCliente.ObterPorId(user.ClienteId, cancellationToken);
                            var tamanhoPastaUtilizado = await _repositoryAmazon.GetFolderSizeInMB(_configuration.GetSection("AmazonS2:ACCESS_KEY_ID").Value, _configuration.GetSection("AmazonS2:SECRET_ACCESS_KEY").Value, _configuration.GetSection("AmazonS2:Bucket").Value, user.ClienteId.ToString());
                            var totalDocumentos = await _repositoryDocumento.CountTotalDocumentosPorUsuario(user.Id, cancellationToken);
                            response.Add(new RelatorioDocumentosResponse()
                            {
                                Titulo = "Total de documentos armazenados",
                                Total = totalDocumentos
                            });
                            response.Add(new RelatorioDocumentosResponse()
                            {
                                Titulo = "GB ocupados",
                                Total = Math.Round(tamanhoPastaUtilizado / 1024, 4)
                            });
                            response.Add(new RelatorioDocumentosResponse()
                            {
                                Titulo = "GB disponíveis",
                                Total = Math.Round((((decimal)cliente.EspacoDisco * 1024) - tamanhoPastaUtilizado) / 1024, 4)
                            });
                            success = true;
                        }
                        break;

                    default:
                        break;
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
