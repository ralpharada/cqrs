using Tellus.Application.Core;
using Tellus.Application.Mapper;
using Tellus.Application.Queries;
using Tellus.Application.Responses;
using Tellus.Core.Events;
using Tellus.Domain.Interfaces;
using MediatR;

namespace Tellus.Application.Handlers
{
    public class ObterTodosIndicePorClienteIdHandler : IRequestHandler<ObterTodosIndicePorClienteIdQuery, IEvent>
    {
        private readonly IIndiceRepository _repository;
        private readonly IClienteRepository _repositoryCliente;
        private readonly UsuarioAutenticado _usuarioAutenticado;

        public ObterTodosIndicePorClienteIdHandler(IIndiceRepository repository, UsuarioAutenticado usuarioAutenticado, IClienteRepository repositoryCliente)
        {
            _repository = repository;
            _repositoryCliente = repositoryCliente;
            _usuarioAutenticado = usuarioAutenticado;
        }
        public async Task<IEvent> Handle(ObterTodosIndicePorClienteIdQuery request, CancellationToken cancellationToken)
        {
            bool success = false;
            var response = new List<IndiceResponse>();
            try
            {
                if (_usuarioAutenticado.Email == null)
                    return new ResultEvent(success, "Acesso expirado");

                var cliente = await _repositoryCliente.ObterPorEmailCadastroAtivo(_usuarioAutenticado.Email, cancellationToken);
                if (cliente != null)
                {
                    var lista = await _repository.ObterTodosPorClienteId(cliente.Id, cancellationToken);
                    response = IndiceMapper<List<IndiceResponse>>.Map(lista);
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
