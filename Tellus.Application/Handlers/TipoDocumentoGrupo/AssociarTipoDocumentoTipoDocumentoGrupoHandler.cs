using Tellus.Application.Core;
using Tellus.Application.Queries;
using Tellus.Core.Events;
using Tellus.Domain.Interfaces;
using Tellus.Domain.Models;
using MediatR;
using Newtonsoft.Json;

namespace Tellus.Application.Handlers
{
    public class AssociarTipoDocumentoTipoDocumentoGrupoHandler : IRequestHandler<AssociarTipoDocumentoTipoDocumentoGrupoQuery, IEvent>
    {
        private readonly ITipoDocumentoGrupoRepository _repositoryTipoDocumentoGrupo;
        private readonly ITipoDocumentoRepository _repositoryTipoDocumento;
        private readonly IClienteRepository _repositoryCliente;
        private readonly UsuarioAutenticado _usuarioAutenticado;
        private readonly ILogClienteRepository _logClienteRepository;

        public AssociarTipoDocumentoTipoDocumentoGrupoHandler(ITipoDocumentoGrupoRepository repositoryTipoDocumentoGrupo, ITipoDocumentoRepository repositoryTipoDocumento, UsuarioAutenticado usuarioAutenticado, IClienteRepository repositoryCliente, ILogClienteRepository logClienteRepository)
        {
            _repositoryTipoDocumentoGrupo = repositoryTipoDocumentoGrupo;
            _repositoryTipoDocumento = repositoryTipoDocumento;
            _repositoryCliente = repositoryCliente;
            _usuarioAutenticado = usuarioAutenticado;
            _logClienteRepository = logClienteRepository;
        }
        public async Task<IEvent> Handle(AssociarTipoDocumentoTipoDocumentoGrupoQuery request, CancellationToken cancellationToken)
        {
            string mensagem = "";
            bool success = false;
            try
            {
                if (_usuarioAutenticado.Email == null)
                    return new ResultEvent(success, "Acesso expirado");

                var cliente = await _repositoryCliente.ObterPorEmailCadastroAtivo(_usuarioAutenticado.Email, cancellationToken);
                if (cliente != null)
                {
                    var tipoDocumentoGrupo = await _repositoryTipoDocumentoGrupo.ObterPorId(request.Id, cliente.Id, cancellationToken);
                    if (tipoDocumentoGrupo != null)
                    {
                        tipoDocumentoGrupo.TipoDocumentoIds = request.TipoDocumentoIds;
                        var tiposDocumentos = _repositoryTipoDocumento.ObterTodosPorIds(tipoDocumentoGrupo.TipoDocumentoIds, cliente.Id);
                        var resultTipoDocumento = await _repositoryTipoDocumentoGrupo.Salvar(tipoDocumentoGrupo, cancellationToken);
                        success = resultTipoDocumento.ModifiedCount > 0;
                        mensagem = success ? "Associação com sucesso!" : "Nenhuma atualização realizada.";
                        if (success)
                            await _logClienteRepository.Salvar(new LogCliente() { Id = Guid.NewGuid(), Pagina = "Associação do Grupo de Documento x Tipo Documento", ClienteId = cliente.Id, Acao = "Grupo de Documento: " + tipoDocumentoGrupo.Nome + "<br/>Tipo Documento: <br/>" + JsonConvert.SerializeObject(tiposDocumentos), DataRegistro = DateTime.UtcNow }, cancellationToken);
                    }
                }
            }
            catch (Exception ex)
            {
                mensagem = ex.Message;
            }
            return new ResultEvent(success, mensagem);
        }

    }
}
