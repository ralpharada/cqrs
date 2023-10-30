using Tellus.Application.Queries;
using Tellus.Core.Events;
using Tellus.Domain.Interfaces;
using MediatR;
using Tellus.Application.Core;

namespace Tellus.Application.Handlers
{
    public class DeletarConsumoClientePorIdHandler : IRequestHandler<DeletarConsumoClientePorIdQuery, IEvent>
    {
        private readonly UsuarioAutenticado _usuarioAutenticado;
        private readonly IContabilizaConsultaRepository _repositoryContabilizaConsulta;
        private readonly IUsuarioAdmRepository _repository;

        public DeletarConsumoClientePorIdHandler(UsuarioAutenticado usuarioAutenticado, IUsuarioAdmRepository repository, IContabilizaConsultaRepository repositoryContabilizaConsulta)
        {
            _repositoryContabilizaConsulta = repositoryContabilizaConsulta;
            _repository = repository;
            _usuarioAutenticado = usuarioAutenticado;
        }

        public async Task<IEvent> Handle(DeletarConsumoClientePorIdQuery request, CancellationToken cancellationToken)
        {
            string mensagem = "";
            var success = false;
            try
            {
                if (_usuarioAutenticado.Email == null)
                    return new ResultEvent(success, "Acesso expirado");

                var usuario = await _repository.ObterPorEmailCadastroAtivo(_usuarioAutenticado.Email, cancellationToken);
                if (usuario != null)
                {
                    success = await _repositoryContabilizaConsulta.DeletarPorId(request.Id, cancellationToken);
                    mensagem = success ? "Consulta excluída com sucesso!" : "Não foi possível excluir a consulta.";
                }
            }
            catch (Exception ex)
            {
                mensagem = ex.Message;
            }
            return new ResultEvent(success, mensagem);
        }

    }
}
