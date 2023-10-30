using Tellus.Application.Core;
using Tellus.Application.Mapper;
using Tellus.Application.Queries;
using Tellus.Application.Responses;
using Tellus.Core.Events;
using Tellus.Domain.Interfaces;
using MediatR;

namespace Tellus.Application.Handlers
{
    public class ObterTipoDocumentoPorIdHandler : IRequestHandler<ObterTipoDocumentoPorIdQuery, IEvent>
    {
        private readonly IIndiceRepository _repository;
        private readonly ITipoDocumentoRepository _repositoryTipoDocumento;
        private readonly IClienteRepository _repositoryCliente;
        private readonly UsuarioAutenticado _usuarioAutenticado;

        public ObterTipoDocumentoPorIdHandler(IIndiceRepository repository, ITipoDocumentoRepository repositoryTipoDocumento, UsuarioAutenticado usuarioAutenticado, IClienteRepository repositoryCliente)
        {
            _repository = repository;
            _repositoryTipoDocumento = repositoryTipoDocumento;
            _repositoryCliente = repositoryCliente;
            _usuarioAutenticado = usuarioAutenticado;
        }
        public async Task<IEvent> Handle(ObterTipoDocumentoPorIdQuery request, CancellationToken cancellationToken)
        {
            bool success = false;
            var response = new TipoDocumentoResponse();
            try
            {
                if (_usuarioAutenticado.Email == null)
                    return new ResultEvent(success, "Acesso expirado");

                var cliente = await _repositoryCliente.ObterPorEmailCadastroAtivo(_usuarioAutenticado.Email, cancellationToken);
                if (cliente != null)
                {
                    var tipoDocumento = await _repositoryTipoDocumento.ObterPorId(request.Id, cliente.Id, cancellationToken);
                    if (tipoDocumento.IndiceIds != null)
                        tipoDocumento.Indices = _repository.ObterTodosPorIds(tipoDocumento.IndiceIds, cliente.Id);
                    response = TipoDocumentoMapper<TipoDocumentoResponse>.Map(tipoDocumento);
                    success = true;
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
