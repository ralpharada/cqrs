using Tellus.Application.Queries;
using Tellus.Core.Events;
using Tellus.Domain.Interfaces;
using MediatR;

namespace Tellus.Application.Handlers
{
    public class ValidarCadastroClienteViaLinkHandler : IRequestHandler<ValidarCadastroClienteViaLinkQuery, IEvent>
    {
        private readonly IClienteRepository _repository;

        public ValidarCadastroClienteViaLinkHandler(IClienteRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEvent> Handle(ValidarCadastroClienteViaLinkQuery request, CancellationToken cancellationToken)
        {
            bool success = false;
            try
            {
                var cliente = await _repository.ObterPorHashAtivacaoCadastro(request.Parametro, cancellationToken);
                success = cliente != null;
                if (success)
                {
                    cliente.DataAtivacaoCadastro = DateTime.UtcNow;
                    cliente.HashAtivacaoCadastro = String.Empty;
                    cliente.DataValidadeAtivacaoCadastro = null;
                    var result = await _repository.Salvar(cliente, cancellationToken);
                    success = result.ModifiedCount > 0;
                }
            }
            catch (Exception ex)
            {
                return new ResultEvent(success, ex.Message);
            }
            return new ResultEvent(success, request.Parametro);
        }

    }
}
