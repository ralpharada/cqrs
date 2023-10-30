using Tellus.Application.Queries;
using Tellus.Core.Events;
using Tellus.Domain.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Tellus.Application.Handlers
{
    public class AtualizarUsuarioLogadoHandler : IRequestHandler<AtualizarUsuarioLogadoQuery, IEvent>
    {
        private readonly IUsuarioLogadoRepository _repository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AtualizarUsuarioLogadoHandler(IUsuarioLogadoRepository repository,
        IHttpContextAccessor httpContextAccessor)
        {
            _repository = repository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<IEvent> Handle(AtualizarUsuarioLogadoQuery request, CancellationToken cancellationToken)
        {
            string mensagem = "";
            var success = false;
            try
            {
                var usuarioLogado = await _repository.ObterPorUsuarioId(request.UsuarioId, cancellationToken);
                if (usuarioLogado != null)
                {
                    var ip = _httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();
                    usuarioLogado.IP = ip;
                    usuarioLogado.UltimaRequisicao = DateTime.UtcNow;
                    success = await _repository.Atualizar(usuarioLogado, cancellationToken);
                }
            }
            catch (Exception ex)
            {
                mensagem = ex.Message;
            }
            return new ResultEvent(success, mensagem, null);
        }

    }
}
