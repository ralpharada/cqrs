
using Tellus.Application.Mapper;
using Tellus.Application.Queries;
using Tellus.Application.Responses;
using Tellus.Core.Events;
using Tellus.Domain.Interfaces;
using MediatR;
using Tellus.Application.Core;

namespace Tellus.Application.Handlers
{
    public class ListarLogHandler : IRequestHandler<ListarLogQuery, IEvent>
    {
        private readonly UsuarioAutenticado _usuarioAutenticado;
        private readonly IUsuarioAdmRepository _repository;
        private readonly ILogRepository _repositoryLog;

        public ListarLogHandler(UsuarioAutenticado usuarioAutenticado, IUsuarioAdmRepository repository, ILogRepository repositoryLog)
        {
            _repository = repository;
            _usuarioAutenticado = usuarioAutenticado;
            _repositoryLog = repositoryLog;
        }

        public async Task<IEvent> Handle(ListarLogQuery request, CancellationToken cancellationToken)
        {
            var success = false;
            long total = 0;
            var response = new List<LogResponse>();
            try
            {
             //   if (_usuarioAutenticado.Email == null)
            //        return new ResultEvent(success, "Acesso expirado");

            //    var usuario = await _repository.ObterPorEmailCadastroAtivo(_usuarioAutenticado.Email, cancellationToken);
           //     if (usuario != null)
           //     {
                    var result = await _repositoryLog.Listar(request.DataDe, request.DataAte, cancellationToken);
                    total = result.Count();
                    response = LogMapper<List<LogResponse>>.Map(result);
                    success = true;
          //      }
            }
            catch (Exception ex)
            {
                return new ResultEvent(success, ex.Message);
            }
            return new ResultEvent(success, response, total);
        }

    }
}
