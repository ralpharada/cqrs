
using Tellus.Application.Mapper;
using Tellus.Application.Queries;
using Tellus.Application.Responses;
using Tellus.Core.Events;
using Tellus.Domain.Interfaces;
using MediatR;
using Tellus.Application.Core;

namespace Tellus.Application.Handlers
{
    public class ObterUsuarioAdmPorIdHandler : IRequestHandler<ObterUsuarioAdmPorIdQuery, IEvent>
    {
        private readonly IUsuarioAdmRepository _repository;
        private readonly UsuarioAutenticado _usuarioAutenticado;

        public ObterUsuarioAdmPorIdHandler(IUsuarioAdmRepository repository, UsuarioAutenticado usuarioAutenticado)
        {
            _repository = repository;
            _usuarioAutenticado = usuarioAutenticado;
        }

        public async Task<IEvent> Handle(ObterUsuarioAdmPorIdQuery request, CancellationToken cancellationToken)
        {
            var success = false;
            var response = new UsuarioAdmResponse();
            try
            {
                if (_usuarioAutenticado.Email == null)
                    return new ResultEvent(success, "Acesso expirado");

                var usuario = await _repository.ObterPorEmailCadastroAtivo(_usuarioAutenticado.Email, cancellationToken);
                if (usuario != null)
                {
                    var taskResult = await _repository.ObterPorId(request.Id, cancellationToken);
                    response = UsuarioAdmMapper<UsuarioAdmResponse>.Map(taskResult);
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
