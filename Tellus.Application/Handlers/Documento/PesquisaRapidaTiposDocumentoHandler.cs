using Tellus.Application.Core;
using Tellus.Application.Mapper;
using Tellus.Application.Queries;
using Tellus.Application.Responses;
using Tellus.Core.Events;
using Tellus.Domain.Interfaces;
using MediatR;

namespace Tellus.Application.Handlers
{
    public class PesquisaRapidaTiposDocumentoHandler : IRequestHandler<PesquisaRapidaTiposDocumentoQuery, IEvent>
    {
        private readonly IDocumentoRepository _repository;
        private readonly ITipoDocumentoRepository _repositoryTipoDocumento;
        private readonly IUsuarioRepository _repositoryUsuario;
        private readonly UsuarioAutenticado _usuarioAutenticado;
        private readonly IClienteRepository _repositoryCliente;
        private readonly IIndiceRepository _repositoryIndice;

        public PesquisaRapidaTiposDocumentoHandler(IDocumentoRepository repository, ITipoDocumentoRepository repositoryTipoDocumento, UsuarioAutenticado usuarioAutenticado, IUsuarioRepository repositoryUsuario, IClienteRepository repositoryCliente, IIndiceRepository repositoryIndice)
        {
            _repository = repository;
            _repositoryTipoDocumento = repositoryTipoDocumento;
            _repositoryUsuario = repositoryUsuario;
            _usuarioAutenticado = usuarioAutenticado;
            _repositoryCliente = repositoryCliente;
            _repositoryIndice = repositoryIndice;
        }
        public async Task<IEvent> Handle(PesquisaRapidaTiposDocumentoQuery request, CancellationToken cancellationToken)
        {
            bool success = false;
            Guid id = Guid.Empty;
            var lista = new List<TipoDocumentoResponse>();
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
                        var pesquisaTiposDocumento = await _repository.PesquisaRapidaTiposDocumento(usuario.ClienteId, request.Pesquisa);
                        var tipoDocumentoList = _repositoryTipoDocumento.ObterTodosPorIds(pesquisaTiposDocumento, usuario.ClienteId);
                        tipoDocumentoList.ForEach(async tipoDocumento =>
                        {
                            tipoDocumento.Indices = _repositoryIndice.ObterTodosPorIds(tipoDocumento.IndiceIds, cliente.Id);
                        });
                        lista = TipoDocumentoMapper<List<TipoDocumentoResponse>>.Map(tipoDocumentoList);
                        success = true;
                    }
                }
            }
            catch (Exception ex)
            {
                return new ResultEvent(success, ex.Message);
            }
            return new ResultEvent(success, lista, total);
        }

    }
}
