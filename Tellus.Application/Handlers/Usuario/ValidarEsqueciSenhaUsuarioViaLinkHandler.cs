using Tellus.Application.Queries;
using Tellus.Core.Events;
using Tellus.Domain.Interfaces;
using MediatR;

namespace Tellus.Application.Handlers
{
    public class ValidarEsqueciSenhaUsuarioViaLinkHandler : IRequestHandler<ValidarEsqueciSenhaUsuarioViaLinkQuery, IEvent>
    {
        private readonly IUsuarioRepository _repository;

        public ValidarEsqueciSenhaUsuarioViaLinkHandler(IUsuarioRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEvent> Handle(ValidarEsqueciSenhaUsuarioViaLinkQuery request, CancellationToken cancellationToken)
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
