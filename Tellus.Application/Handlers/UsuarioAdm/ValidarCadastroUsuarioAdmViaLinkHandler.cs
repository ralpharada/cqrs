using Tellus.Application.Queries;
using Tellus.Core.Events;
using Tellus.Domain.Interfaces;
using MediatR;

namespace Tellus.Application.Handlers
{
    public class ValidarCadastroUsuarioAdmViaLinkHandler : IRequestHandler<ValidarCadastroUsuarioAdmViaLinkQuery, IEvent>
    {
        private readonly IUsuarioAdmRepository _repository;

        public ValidarCadastroUsuarioAdmViaLinkHandler(IUsuarioAdmRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEvent> Handle(ValidarCadastroUsuarioAdmViaLinkQuery request, CancellationToken cancellationToken)
        {
            bool success = false;
            try
            {
                success = await _repository.ObterPorHashAtivacaoCadastro(request.Parametro, cancellationToken) != null;
            }
            catch (Exception ex)
            {
                return new ResultEvent(success, ex.Message);
            }
            return new ResultEvent(success, request.Parametro);
        }

    }
}
