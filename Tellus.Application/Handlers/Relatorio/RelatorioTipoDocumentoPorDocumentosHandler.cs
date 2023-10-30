using Tellus.Application.Core;
using Tellus.Application.Queries;
using Tellus.Application.Responses;
using Tellus.Core.Events;
using Tellus.Domain.Interfaces;
using MediatR;

namespace Tellus.Application.Handlers
{
    public class RelatorioTipoDocumentoPorDocumentosHandler : IRequestHandler<RelatorioTipoDocumentoPorDocumentosQuery, IEvent>
    {
        private readonly IClienteRepository _repositoryCliente;
        private readonly IUsuarioRepository _repositoryUsuario;
        private readonly UsuarioAutenticado _usuarioAutenticado;
        private readonly IDocumentoRepository _repositoryDocumento;
        private readonly ITipoDocumentoRepository _repositoryTipoDocumento;

        public RelatorioTipoDocumentoPorDocumentosHandler(IClienteRepository repositoryCliente,
            UsuarioAutenticado usuarioAutenticado,
            IDocumentoRepository repositoryDocumento,
            ITipoDocumentoRepository repositoryTipoDocumento,
            IUsuarioRepository repositoryUsuario)
        {
            _repositoryCliente = repositoryCliente;
            _usuarioAutenticado = usuarioAutenticado;
            _repositoryDocumento = repositoryDocumento;
            _repositoryTipoDocumento = repositoryTipoDocumento;
            _repositoryUsuario = repositoryUsuario;

        }

        public async Task<IEvent> Handle(RelatorioTipoDocumentoPorDocumentosQuery request, CancellationToken cancellationToken)
        {
            var success = false;
            var response = new RelatorioResponse();
            try
            {
                if (_usuarioAutenticado.Email == null)
                    return new ResultEvent(success, "Acesso expirado");
                switch (request.Tipo)
                {
                    case "client":
                        var cliente = await _repositoryCliente.ObterPorEmailCadastroAtivo(_usuarioAutenticado.Email, cancellationToken);
                        if (cliente != null)
                        {

                            var listaTiposDocumentos = await _repositoryTipoDocumento.ObterTodosCompleto(cliente.Id, cancellationToken);
                            var listaDocumentos = await _repositoryDocumento.ObterPorCliente(cliente.Id, cancellationToken);
                            List<string> sb = new List<string>();
                            foreach (var tipoDocumento in listaTiposDocumentos)
                            {
                                sb.Add($"\"{tipoDocumento.Nome}\":{listaDocumentos.Count(x => x.TipoDocumentoId == tipoDocumento.Id)}");
                            }
                            response = new RelatorioResponse()
                            {
                                Titulo = "Quantidade de documentos por tipo de documentos",
                                Tipo = "Array",
                                Json = $"{{{String.Join(",", sb)}}}"
                            };
                            success = true;
                        }
                        break;
                    case "user":
                        var user = await _repositoryUsuario.ObterPorEmailCadastroAtivo(_usuarioAutenticado.Email, cancellationToken);
                        if (user != null)
                        {

                            var listaTiposDocumentos = _repositoryTipoDocumento.ObterTodosPorIds(user.TipoDocumentoIds, user.ClienteId);
                            var listaDocumentos = await _repositoryDocumento.ObterPorUsuario(user.Id, cancellationToken);
                            List<string> sb = new List<string>();
                            foreach (var tipoDocumento in listaTiposDocumentos)
                            {
                                sb.Add($"\"{tipoDocumento.Nome}\":{listaDocumentos.Count(x => x.TipoDocumentoId == tipoDocumento.Id)}");
                            }
                            response = new RelatorioResponse()
                            {
                                Titulo = "Quantidade de documentos por tipo de documentos",
                                Tipo = "Array",
                                Json = $"{{{String.Join(",", sb)}}}"
                            };
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
