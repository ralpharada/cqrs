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
    public class ObterTodosUsuarioAdmHandler : IRequestHandler<ObterTodosUsuarioAdmQuery, IEvent>
    {
        private readonly IUsuarioAdmRepository _repository;
        private readonly IConfiguration _configuration;
        private readonly UsuarioAutenticado _usuarioAutenticado;

        public ObterTodosUsuarioAdmHandler(IUsuarioAdmRepository repository, IConfiguration configuration, UsuarioAutenticado usuarioAutenticado)
        {
            _repository = repository;
            _configuration = configuration;
            _usuarioAutenticado = usuarioAutenticado;
        }

        public async Task<IEvent> Handle(ObterTodosUsuarioAdmQuery request, CancellationToken cancellationToken)
        {
            var success = false;
            var response = new List<UsuarioAdmResponse>();
            long total = 0;
            try
            {
                if (_usuarioAutenticado.Email == null)
                    return new ResultEvent(success, "Acesso expirado");

                var usuario = await _repository.ObterPorEmailCadastroAtivo(_usuarioAutenticado.Email, cancellationToken);
                if (usuario != null)
                {
                    var lista = await _repository.ObterTodos(request.Filtro, request.CurrentPage, Convert.ToInt32(_configuration.GetSection("PageSize").Value), cancellationToken);
                    total = await _repository.Count(cancellationToken);
                    response = UsuarioAdmMapper<List<UsuarioAdmResponse>>.Map(lista);
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
