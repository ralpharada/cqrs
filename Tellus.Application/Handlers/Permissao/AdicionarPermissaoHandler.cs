using Tellus.Application.Core;
using Tellus.Application.Queries;
using Tellus.Core.Events;
using Tellus.Domain.Interfaces;
using Tellus.Domain.Models;
using MediatR;

namespace Tellus.Application.Handlers
{
    public class AdicionarPermissaoHandler : IRequestHandler<AdicionarPermissaoQuery, IEvent>
    {
        private readonly IPermissaoRepository _repository;
        private readonly IUsuarioAdmRepository _repositoryUsuarioAdm;
        private readonly UsuarioAutenticado _usuarioAutenticado;

        public AdicionarPermissaoHandler(IPermissaoRepository repository, UsuarioAutenticado usuarioAutenticado, IUsuarioAdmRepository repositoryUsuarioAdm)
        {
            _repository = repository;
            _repositoryUsuarioAdm = repositoryUsuarioAdm;
            _usuarioAutenticado = usuarioAutenticado;
        }
        public async Task<IEvent> Handle(AdicionarPermissaoQuery request, CancellationToken cancellationToken)
        {
            string mensagem = "";
            bool success = false;
            Guid id = Guid.Empty;
            try
            {
                if (_usuarioAutenticado.Email == null)
                    return new ResultEvent(success, "Acesso expirado");

                var usuarioAdm = await _repositoryUsuarioAdm.ObterPorEmailCadastroAtivo(_usuarioAutenticado.Email, cancellationToken);
                if (usuarioAdm != null)
                {
                    var Permissao = new Permissao
                    {
                        Id = Guid.NewGuid(),
                        Nome = request.Nome,
                        VinculoId = request.VinculoId,
                        Status = request.Status
                    };
                    var result = await _repository.Salvar(Permissao, cancellationToken);
                    success = result.UpsertedId > 0;
                    if (success)
                    {
                        id = ((Guid)result.UpsertedId);
                        mensagem = "Permissão cadastrada com sucesso!";
                    }
                }
            }
            catch (Exception ex)
            {
                mensagem = ex.Message;
            }
            return new ResultEvent(success, mensagem, null, id);
        }

    }
}
