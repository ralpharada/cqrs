using Tellus.Application.Mapper;
using Tellus.Application.Queries;
using Tellus.Application.Responses;
using Tellus.Core.Events;
using Tellus.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Configuration;
using Tellus.Application.Core;

namespace Tellus.Application.Handlers
{
    public class ObterTodosClienteHandler : IRequestHandler<ObterTodosClienteQuery, IEvent>
    {
        private readonly UsuarioAutenticado _usuarioAutenticado;
        private readonly IClienteRepository _repositoryCliente;
        private readonly IUsuarioAdmRepository _repository;
        private readonly IConfiguration _configuration;

        public ObterTodosClienteHandler(IClienteRepository repositoryCliente, UsuarioAutenticado usuarioAutenticado, IUsuarioAdmRepository repository, IConfiguration configuration)
        {
            _repositoryCliente = repositoryCliente;
            _repository = repository;
            _usuarioAutenticado = usuarioAutenticado;
            _configuration = configuration;
        }

        public async Task<IEvent> Handle(ObterTodosClienteQuery request, CancellationToken cancellationToken)
        {
            var success = false;
            var response = new List<ClienteResponse>();
            long total = 0;
            try
            {
                if (_usuarioAutenticado.Email == null)
                    return new ResultEvent(success, "Acesso expirado");

                var usuario = await _repository.ObterPorEmailCadastroAtivo(_usuarioAutenticado.Email, cancellationToken);
                if (usuario != null)
                {
                    var lista = await _repositoryCliente.ObterTodos(request.Filtro, request.CurrentPage, Convert.ToInt32(_configuration.GetSection("PageSize").Value), cancellationToken);
                    total = await _repository.Count(cancellationToken);
                    response = ClienteMapper<List<ClienteResponse>>.Map(lista);
                    success = true;
                }
            }
            catch (Exception ex)
            {
                return new ResultEvent(success, ex.Message);
            }
            return new ResultEvent(success, response, total);
        }
    }
}
