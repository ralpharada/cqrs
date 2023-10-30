using Tellus.Application.Core;
using Tellus.Application.Mapper;
using Tellus.Application.Queries;
using Tellus.Application.Responses;
using Tellus.Core.Events;
using Tellus.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Configuration;

namespace Tellus.Application.Handlers
{
    public class ObterTodosUsuarioPorClienteIdHandler : IRequestHandler<ObterTodosUsuarioPorClienteIdQuery, IEvent>
    {
        private readonly IUsuarioRepository _repository;
        private readonly IClienteRepository _repositoryCliente;
        private readonly UsuarioAutenticado _usuarioAutenticado;
        private readonly IConfiguration _configuration;

        public ObterTodosUsuarioPorClienteIdHandler(IUsuarioRepository repository, IClienteRepository repositoryCliente, UsuarioAutenticado usuarioAutenticado, IConfiguration configuration)
        {
            _repository = repository;
            _repositoryCliente = repositoryCliente;
            _configuration = configuration;
            _usuarioAutenticado = usuarioAutenticado;
        }

        public async Task<IEvent> Handle(ObterTodosUsuarioPorClienteIdQuery request, CancellationToken cancellationToken)
        {
            var success = false;
            var response = new List<UsuarioResponse>();
            long total = 0;
            try
            {
                if (_usuarioAutenticado.Email == null)
                    return new ResultEvent(success, "Acesso expirado");

                var cliente = await _repositoryCliente.ObterPorEmailCadastroAtivo(_usuarioAutenticado.Email, cancellationToken);
                if (cliente != null)
                {
                    var lista = await _repository.ObterTodosPorClienteId(cliente.Id,request.Filtro, request.CurrentPage, Convert.ToInt32(_configuration.GetSection("PageSize").Value), cancellationToken);
                    total = await _repository.Count(cliente.Id, request.Filtro, cancellationToken);
                    response = UsuarioMapper<List<UsuarioResponse>>.Map(lista);
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
