using Tellus.Application.Core;
using Tellus.Application.Queries;
using Tellus.Application.Responses;
using Tellus.Core.Events;
using Tellus.Domain.Interfaces;
using MediatR;

namespace Tellus.Application.Handlers
{
    public class RelatorioDocumentosUltimos12MesesHandler : IRequestHandler<RelatorioDocumentosUltimos12MesesQuery, IEvent>
    {
        private readonly IClienteRepository _repositoryCliente;
        private readonly IUsuarioRepository _repositoryUsuario;
        private readonly UsuarioAutenticado _usuarioAutenticado;
        private readonly IDocumentoRepository _repositoryDocumento;
        private readonly ITipoDocumentoRepository _repositoryTipoDocumento;

        public RelatorioDocumentosUltimos12MesesHandler(IClienteRepository repositoryCliente,
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

        public async Task<IEvent> Handle(RelatorioDocumentosUltimos12MesesQuery request, CancellationToken cancellationToken)
        {
            var success = false;
            var response = new RelatorioResponse();
            try
            {
                if (_usuarioAutenticado.Email == null)
                    return new ResultEvent(success, "Acesso expirado");
                var data = DateTime.Now;
                data.AddMonths(-12);
                data = new DateTime(data.Year, data.Month, 1);
                switch (request.Tipo)
                {
                    case "client":
                        var cliente = await _repositoryCliente.ObterPorEmailCadastroAtivo(_usuarioAutenticado.Email, cancellationToken);
                        if (cliente != null)
                        {

                            var listaTiposDocumentos = await _repositoryTipoDocumento.ObterTodosCompleto(cliente.Id, cancellationToken);
                            var listaDocumentosDataAte = await _repositoryDocumento.DocumentoPorClienteDataDe(cliente.Id, data.ToShortDateString(), cancellationToken);
                            var resultado = listaDocumentosDataAte.GroupBy(x => new { x.DataCadastro.Year, x.DataCadastro.Month }).Select(g => new
                            {
                                Ano = g.Key.Year,
                                Mês = g.Key.Month,
                                Total = g.Count()
                            })
                           .OrderByDescending(g => g.Ano)
                           .ThenByDescending(g => g.Mês);
                            List<string> sb = new List<string>();
                            foreach (var item in resultado)
                            {
                                sb.Add($"\"{item.Mês}/{item.Ano}\":{item.Total}");
                            }
                            response = new RelatorioResponse()
                            {
                                Titulo = "Quantidade de documentos nos últimos 12 meses",
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
                            var listaDocumentosDataAte = await _repositoryDocumento.DocumentoPorUsuarioDataDe(user.Id, data.ToShortDateString(), cancellationToken);
                            var resultado = listaDocumentosDataAte.GroupBy(x => new { x.DataCadastro.Year, x.DataCadastro.Month }).Select(g => new
                            {
                                Ano = g.Key.Year,
                                Mês = g.Key.Month,
                                Total = g.Count()
                            })
                           .OrderByDescending(g => g.Ano)
                           .ThenByDescending(g => g.Mês);
                            List<string> sb = new List<string>();
                            foreach (var item in resultado)
                            {
                                sb.Add($"\"{item.Mês}/{item.Ano}\":{item.Total}");
                            }
                            response = new RelatorioResponse()
                            {
                                Titulo = "Quantidade de documentos nos últimos 12 meses",
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
