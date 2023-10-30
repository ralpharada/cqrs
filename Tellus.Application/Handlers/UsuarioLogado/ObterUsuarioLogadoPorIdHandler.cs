
using Tellus.Application.Core;
using Tellus.Application.Mapper;
using Tellus.Application.Queries;
using Tellus.Application.Responses;
using Tellus.Core.Events;
using Tellus.Domain.Interfaces;
using MediatR;

namespace Tellus.Application.Handlers
{
    public class ObterUsuarioLogadoPorIdHandler : IRequestHandler<ObterUsuarioLogadoPorIdQuery, IEvent>
    {
        private readonly IUsuarioRepository _repository;
        private readonly ITipoDocumentoRepository _repositoryTipoDocumento;
        private readonly IClienteRepository _repositoryCliente;
        private readonly UsuarioAutenticado _usuarioAutenticado;

        public ObterUsuarioLogadoPorIdHandler(IUsuarioRepository repository, UsuarioAutenticado usuarioAutenticado, IClienteRepository repositoryCliente, ITipoDocumentoRepository repositoryTipoDocumento)
        {
            _repository = repository;
            _repositoryTipoDocumento = repositoryTipoDocumento;
            _repositoryCliente = repositoryCliente;
            _usuarioAutenticado = usuarioAutenticado;
        }

        public async Task<IEvent> Handle(ObterUsuarioLogadoPorIdQuery request, CancellationToken cancellationToken)
        {
            var success = false;
            var response = new UsuarioResponse();
            try
            {
                if (_usuarioAutenticado.Email == null)
                    return new ResultEvent(success, "Acesso expirado");

                var cliente = await _repositoryCliente.ObterPorEmailCadastroAtivo(_usuarioAutenticado.Email, cancellationToken);
                if (cliente != null)
                {
                    var taskResult = await _repository.ObterPorId(request.Id, cancellationToken);
                    if (taskResult != null)
                    {
                        if (taskResult.TipoDocumentoIds != null)
                            taskResult.TipoDocumentos = _repositoryTipoDocumento.ObterTodosPorIds(taskResult.TipoDocumentoIds, cliente.Id);
                        response = UsuarioMapper<UsuarioResponse>.Map(taskResult);
                        success = true;
                    }
                    else
                        return new ResultEvent(success, "Usuário não localizado");
                }
                else
                    return new ResultEvent(success, "Conta não localizada");
            }
            catch (Exception ex)
            {
                return new ResultEvent(success, ex.Message);
            }
            return new ResultEvent(success, response);
        }

    }
}
