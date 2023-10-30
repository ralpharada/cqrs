using Tellus.Application.Queries;
using Tellus.Core.Events;
using Tellus.Domain.Interfaces;
using MediatR;

namespace Tellus.Application.Handlers
{
    public class ValidarEsqueciSenhaUsuarioAdmViaLinkHandler : IRequestHandler<ValidarEsqueciSenhaUsuarioAdmViaLinkQuery, IEvent>
    {
        private readonly IUsuarioAdmRepository _repository;

        public ValidarEsqueciSenhaUsuarioAdmViaLinkHandler(IUsuarioAdmRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEvent> Handle(ValidarEsqueciSenhaUsuarioAdmViaLinkQuery request, CancellationToken cancellationToken)
        {
            bool success = false;
            try
            {
                success = await _repository.ObterPorHashEsqueciSenha(request.Parametro, cancellationToken) != null;
            }
            catch (Exception ex)
            {
                return new ResultEvent(success, ex.Message);
            }
            return new ResultEvent(success, null);
        }

    }
}
