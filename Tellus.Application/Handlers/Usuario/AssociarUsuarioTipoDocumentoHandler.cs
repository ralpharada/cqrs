using Tellus.Application.Core;
using Tellus.Application.Queries;
using Tellus.Core.Events;
using Tellus.Domain.Interfaces;
using Tellus.Domain.Models;
using MediatR;
using Newtonsoft.Json;

namespace Tellus.Application.Handlers
{
    public class AssociarUsuarioTipoDocumentoHandler : IRequestHandler<AssociarUsuarioTipoDocumentoQuery, IEvent>
    {
        private readonly UsuarioAutenticado _usuarioAutenticado;
        private readonly IClienteRepository _repositoryCliente;
        private readonly IUsuarioRepository _repository;
        private readonly ILogClienteRepository _logClienteRepository;
        private readonly ITipoDocumentoRepository _tipoDocumentoRepository;

        public AssociarUsuarioTipoDocumentoHandler(IClienteRepository repositoryCliente, UsuarioAutenticado usuarioAutenticado, IUsuarioRepository repository, ILogClienteRepository logClienteRepository, ITipoDocumentoRepository tipoDocumentoRepository)
        {
            _repositoryCliente = repositoryCliente;
            _repository = repository;
            _usuarioAutenticado = usuarioAutenticado;
            _logClienteRepository = logClienteRepository;
            _tipoDocumentoRepository = tipoDocumentoRepository;
        }
        public async Task<IEvent> Handle(AssociarUsuarioTipoDocumentoQuery request, CancellationToken cancellationToken)
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
                    var usuario = await _repository.ObterPorId(request.Id, cancellationToken);
                    if (usuario != null)
                    {
                        usuario.TipoDocumentoIds = request.TipoDocumentoIds;
                        var tiposDocumentos = _tipoDocumentoRepository.ObterTodosPorIds(usuario.TipoDocumentoIds, cliente.Id);
                        if (usuario.VinculoPermissoes != null)
                        {
                            var itemsToRemove = new List<VinculoPermissao>();
                            foreach (var item in usuario.VinculoPermissoes)
                            {
                                if (!usuario.TipoDocumentoIds.Exists(x => x == item.TipoDocumentoId))
                                {
                                    itemsToRemove.Add(item);
                                }
                            }
                            foreach (var itemToRemove in itemsToRemove)
                            {
                                usuario.VinculoPermissoes.Remove(itemToRemove);
                            }
                        }
                        var resultTipoDocumento = await _repository.Salvar(usuario, cancellationToken);
                        success = resultTipoDocumento.ModifiedCount > 0;
                        mensagem = success ? "Associação com sucesso!" : "Nenhuma atualização realizada.";
                        if (success)
                            await _logClienteRepository.Salvar(new LogCliente() { Id = Guid.NewGuid(), Pagina = "Associação do Usuário x Tipo Documento", ClienteId = cliente.Id, Acao = "Usuário: " + usuario.Nome + "<br/>Tipo Documento: <br/>" + JsonConvert.SerializeObject(tiposDocumentos.Select(x => x.Nome)), DataRegistro = DateTime.UtcNow }, cancellationToken);
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
