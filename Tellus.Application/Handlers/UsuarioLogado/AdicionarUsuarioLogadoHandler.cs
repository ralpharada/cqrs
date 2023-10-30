using Tellus.Application.Queries;
using Tellus.Core.Events;
using Tellus.Domain.Interfaces;
using Tellus.Domain.Models;
using MediatR;

namespace Tellus.Application.Handlers
{
    public class AdicionarUsuarioLogadoHandler : IRequestHandler<AdicionarUsuarioLogadoQuery, IEvent>
    {
        private readonly IUsuarioLogadoRepository _repository;

        public AdicionarUsuarioLogadoHandler(IUsuarioLogadoRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEvent> Handle(AdicionarUsuarioLogadoQuery request, CancellationToken cancellationToken)
        {
            string mensagem = "";
            var success = false;
            try
            {
                var usuarioLogado = new UsuarioLogado() { UsuarioId = request.UsuarioId, ClienteId = request.ClienteId, Id = Guid.NewGuid(), UltimaRequisicao = DateTime.UtcNow, IP = request.IP };
                var excluido = await _repository.ExcluirPorUsuarioId(request.UsuarioId, cancellationToken);
                await _repository.ExcluirVencidosPorClienteId(request.ClienteId, cancellationToken);
                var totalAtivos = await _repository.CountPorCliente(request.ClienteId, cancellationToken);
                if (totalAtivos >= request.QtdeUsuarios)
                {
                    mensagem = "Limite excedido de usuários logados.";
                }
                else
                {
                    await _repository.Adicionar(usuarioLogado, cancellationToken);
                    success = true;
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
