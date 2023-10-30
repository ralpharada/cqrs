
using Tellus.Application.Mapper;
using Tellus.Application.Queries;
using Tellus.Application.Responses;
using Tellus.Core.Events;
using Tellus.Domain.Interfaces;
using MediatR;
using Tellus.Application.Core;

namespace Tellus.Application.Handlers
{
    public class ObterConsumoClienteHandler : IRequestHandler<ObterConsumoClienteQuery, IEvent>
    {
        private readonly UsuarioAutenticado _usuarioAutenticado;
        private readonly IUsuarioAdmRepository _repository;
        private readonly IContabilizaConsultaRepository _repositoryContabiliza;

        public ObterConsumoClienteHandler(UsuarioAutenticado usuarioAutenticado, IUsuarioAdmRepository repository, IContabilizaConsultaRepository repositoryContabiliza)
        {
            _repository = repository;
            _usuarioAutenticado = usuarioAutenticado;
            _repositoryContabiliza = repositoryContabiliza;
        }

        public async Task<IEvent> Handle(ObterConsumoClienteQuery request, CancellationToken cancellationToken)
        {
            var success = false;
            long total = 0;
            var response = new List<ContabilizaConsultaResponse>();
            try
            {
                if (_usuarioAutenticado.Email == null)
                    return new ResultEvent(success, "Acesso expirado");

                var usuario = await _repository.ObterPorEmailCadastroAtivo(_usuarioAutenticado.Email, cancellationToken);
                if (usuario != null)
                {

                    var result = await _repositoryContabiliza.ObterFiltro(request.Id, request.ProdutoId, request.DataDe, request.DataAte, cancellationToken);
                    total = result.Count();
                    response = ContabilizaConsultaMapper<List<ContabilizaConsultaResponse>>.Map(result);
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
