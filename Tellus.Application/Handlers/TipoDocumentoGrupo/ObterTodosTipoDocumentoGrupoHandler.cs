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
    public class ObterTodosTipoDocumentoGrupoHandler : IRequestHandler<ObterTodosTipoDocumentoGrupoQuery, IEvent>
    {
        private readonly ITipoDocumentoGrupoRepository _repository;
        private readonly IConfiguration _configuration;
        private readonly IClienteRepository _repositoryCliente;
        private readonly UsuarioAutenticado _usuarioAutenticado;

        public ObterTodosTipoDocumentoGrupoHandler(ITipoDocumentoGrupoRepository repository, IConfiguration configuration, UsuarioAutenticado usuarioAutenticado, IClienteRepository repositoryCliente)
        {
            _repository = repository;
            _configuration = configuration;
            _repositoryCliente = repositoryCliente;
            _usuarioAutenticado = usuarioAutenticado;
        }

        public async Task<IEvent> Handle(ObterTodosTipoDocumentoGrupoQuery request, CancellationToken cancellationToken)
        {
            bool success = false;
            var response = new List<TipoDocumentoGrupoResponse>();
            long total = 0;
            try
            {
                if (_usuarioAutenticado.Email == null)
                    return new ResultEvent(success, "Acesso expirado");
                var cliente = await _repositoryCliente.ObterPorEmailCadastroAtivo(_usuarioAutenticado.Email, cancellationToken);
                if (cliente != null)
                {
                    var taskResult = await _repository.ObterTodos(cliente.Id, request.Filtro, request.CurrentPage, Convert.ToInt32(_configuration.GetSection("PageSize").Value), cancellationToken);
                    total = await _repository.Count(cliente.Id, cancellationToken);
                    response = TipoDocumentoGrupoMapper<List<TipoDocumentoGrupoResponse>>.Map(taskResult);
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
