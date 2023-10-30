using Tellus.Application.Core;
using Tellus.Application.Queries;
using Tellus.Core.Events;
using Tellus.Domain.Interfaces;
using MediatR;

namespace Tellus.Application.Handlers
{
    public class AtualizarVinculoHandler : IRequestHandler<AtualizarVinculoQuery, IEvent>
    {
        private readonly IVinculoRepository _repository;
        private readonly IUsuarioAdmRepository _repositoryUsuarioAdm;
        private readonly UsuarioAutenticado _usuarioAutenticado;

        public AtualizarVinculoHandler(IVinculoRepository repository, UsuarioAutenticado usuarioAutenticado, IUsuarioAdmRepository repositoryUsuarioAdm)
        {
            _repository = repository;
            _repositoryUsuarioAdm = repositoryUsuarioAdm;
            _usuarioAutenticado = usuarioAutenticado;
        }
        public async Task<IEvent> Handle(AtualizarVinculoQuery request, CancellationToken cancellationToken)
        {
            string mensagem = "";
            bool success = false;
            try
            {
                if (_usuarioAutenticado.Email == null)
                    return new ResultEvent(success, "Acesso expirado");

                var cliente = await _repositoryUsuarioAdm.ObterPorEmailCadastroAtivo(_usuarioAutenticado.Email, cancellationToken);
                if (cliente != null)
                {
                    var vinculo = await _repository.ObterPorId(request.Id, cancellationToken);
                    if (vinculo != null)
                    {
                        vinculo.Nome = request.Nome;
                        vinculo.Status = request.Status;
                        var result = await _repository.Salvar(vinculo, cancellationToken);
                        success = result.ModifiedCount > 0;
                        mensagem = success ? "Vinculo atualizado com sucesso!" : "Nenhuma atualização realizada.";
                    }
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
