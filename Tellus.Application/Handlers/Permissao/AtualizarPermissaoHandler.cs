using Tellus.Application.Core;
using Tellus.Application.Queries;
using Tellus.Core.Events;
using Tellus.Domain.Interfaces;
using MediatR;

namespace Tellus.Application.Handlers
{
    public class AtualizarPermissaoHandler : IRequestHandler<AtualizarPermissaoQuery, IEvent>
    {
        private readonly IPermissaoRepository _repository;
        private readonly IUsuarioAdmRepository _repositoryUsuarioAdm;
        private readonly UsuarioAutenticado _usuarioAutenticado;

        public AtualizarPermissaoHandler(IPermissaoRepository repository, UsuarioAutenticado usuarioAutenticado, IUsuarioAdmRepository repositoryUsuarioAdm)
        {
            _repository = repository;
            _repositoryUsuarioAdm = repositoryUsuarioAdm;
            _usuarioAutenticado = usuarioAutenticado;
        }
        public async Task<IEvent> Handle(AtualizarPermissaoQuery request, CancellationToken cancellationToken)
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
                    var permissao = await _repository.ObterPorId(request.Id, cancellationToken);
                    if (permissao != null)
                    {
                        permissao.Nome = request.Nome;
                        permissao.VinculoId = request.VinculoId;
                        permissao.Status = request.Status;
                        var result = await _repository.Salvar(permissao, cancellationToken);
                        success = result.ModifiedCount > 0;
                        mensagem = success ? "Permissão atualizada com sucesso!" : "Nenhuma atualização realizada.";
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
