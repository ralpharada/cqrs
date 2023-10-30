using Tellus.Application.Core;
using Tellus.Application.Queries;
using Tellus.Core.Events;
using Tellus.Domain.Interfaces;
using Tellus.Domain.Models;
using MediatR;

namespace Tellus.Application.Handlers
{
    public class AdicionarVinculoHandler : IRequestHandler<AdicionarVinculoQuery, IEvent>
    {
        private readonly IVinculoRepository _repository;
        private readonly IUsuarioAdmRepository _repositoryUsuarioAdm;
        private readonly UsuarioAutenticado _usuarioAutenticado;

        public AdicionarVinculoHandler(IVinculoRepository repository, UsuarioAutenticado usuarioAutenticado, IUsuarioAdmRepository repositoryUsuarioAdm)
        {
            _repository = repository;
            _repositoryUsuarioAdm = repositoryUsuarioAdm;
            _usuarioAutenticado = usuarioAutenticado;
        }
        public async Task<IEvent> Handle(AdicionarVinculoQuery request, CancellationToken cancellationToken)
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
                    var vinculo = new Vinculo
                    {
                        Id = Guid.NewGuid(),
                        Nome = request.Nome,
                        Status = request.Status
                    };
                    var result = await _repository.Salvar(vinculo, cancellationToken);
                    success = result.UpsertedId > 0;
                    if (success)
                    {
                        id = ((Guid)result.UpsertedId);
                        mensagem = "Vinculo cadastrado com sucesso!";
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
