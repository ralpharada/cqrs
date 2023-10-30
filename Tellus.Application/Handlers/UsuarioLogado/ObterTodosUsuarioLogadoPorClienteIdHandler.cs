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
    public class ObterTodosUsuarioLogadoPorClienteIdHandler : IRequestHandler<ObterTodosUsuarioLogadoPorClienteIdQuery, IEvent>
    {
        private readonly IUsuarioLogadoRepository _repository;
        private readonly IUsuarioRepository _repositoryUsuario;
        private readonly IClienteRepository _repositoryCliente;
        private readonly UsuarioAutenticado _usuarioAutenticado;
        private readonly IConfiguration _configuration;

        public ObterTodosUsuarioLogadoPorClienteIdHandler(IUsuarioLogadoRepository repository, IUsuarioRepository repositoryUsuario, IClienteRepository repositoryCliente, UsuarioAutenticado usuarioAutenticado, IConfiguration configuration)
        {
            _repository = repository;
            _repositoryUsuario = repositoryUsuario;
            _repositoryCliente = repositoryCliente;
            _configuration = configuration;
            _usuarioAutenticado = usuarioAutenticado;
        }

        public async Task<IEvent> Handle(ObterTodosUsuarioLogadoPorClienteIdQuery request, CancellationToken cancellationToken)
        {
            var success = false;
            var response = new List<UsuarioLogadoResponse>();
            long total = 0;
            try
            {
                if (_usuarioAutenticado.Email == null)
                    return new ResultEvent(success, "Acesso expirado");

                var cliente = await _repositoryCliente.ObterPorEmailCadastroAtivo(_usuarioAutenticado.Email, cancellationToken);
                if (cliente != null)
                {
                    await _repository.ExcluirVencidosPorClienteId(cliente.Id, cancellationToken);
                    var lista = await _repository.ObterPorCliente(cliente.Id, request.CurrentPage, Convert.ToInt32(_configuration.GetSection("PageSize").Value), cancellationToken);
                    var usuarios = await _repositoryUsuario.ObterTodosCompleto(cliente.Id, cancellationToken);
                    total = await _repository.CountPorCliente(cliente.Id, cancellationToken);

                    response = UsuarioLogadoMapper<List<UsuarioLogadoResponse>>.Map(lista);
                    response.ForEach(x =>
                    {
                        x.UsuarioNome = usuarios.FirstOrDefault(u => u.Id == x.UsuarioId).Nome;
                    });
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
