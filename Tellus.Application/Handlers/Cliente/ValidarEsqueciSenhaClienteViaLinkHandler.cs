using Tellus.Application.Queries;
using Tellus.Core.Events;
using Tellus.Domain.Interfaces;
using MediatR;

namespace Tellus.Application.Handlers
{
    public class ValidarEsqueciSenhaClienteViaLinkHandler : IRequestHandler<ValidarEsqueciSenhaClienteViaLinkQuery, IEvent>
    {
        private readonly IClienteRepository _repository;

        public ValidarEsqueciSenhaClienteViaLinkHandler(IClienteRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEvent> Handle(ValidarEsqueciSenhaClienteViaLinkQuery request, CancellationToken cancellationToken)
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
