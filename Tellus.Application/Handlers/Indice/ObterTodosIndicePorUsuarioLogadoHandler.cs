using Tellus.Application.Core;
using Tellus.Application.Mapper;
using Tellus.Application.Queries;
using Tellus.Application.Responses;
using Tellus.Core.Events;
using Tellus.Domain.Interfaces;
using MediatR;

namespace Tellus.Application.Handlers
{
    public class ObterTodosIndicePorUsuarioLogadoHandler : IRequestHandler<ObterTodosIndicePorUsuarioLogadoQuery, IEvent>
    {
        private readonly IIndiceRepository _repository;
        private readonly IUsuarioRepository _repositoryUsuario;
        private readonly UsuarioAutenticado _usuarioAutenticado;
        private readonly IClienteRepository _repositoryCliente;

        public ObterTodosIndicePorUsuarioLogadoHandler(IIndiceRepository repository, UsuarioAutenticado usuarioAutenticado, IUsuarioRepository repositoryUsuario, IClienteRepository repositoryCliente)
        {
            _repository = repository;
            _repositoryUsuario = repositoryUsuario;
            _usuarioAutenticado = usuarioAutenticado;
            _repositoryCliente = repositoryCliente;
        }
        public async Task<IEvent> Handle(ObterTodosIndicePorUsuarioLogadoQuery request, CancellationToken cancellationToken)
        {
            bool success = false;
            var response = new List<IndiceResponse>();
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
                        var lista = await _repository.ObterTodosPorClienteId(usuario.ClienteId, cancellationToken);
                        response = IndiceMapper<List<IndiceResponse>>.Map(lista);
                        success = true;
                    }
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
