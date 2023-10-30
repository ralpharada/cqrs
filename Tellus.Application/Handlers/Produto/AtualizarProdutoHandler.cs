using Tellus.Application.Queries;
using Tellus.Core.Events;
using Tellus.Domain.Interfaces;
using Tellus.Domain.Models;
using MediatR;

namespace Tellus.Application.Handlers
{
    public class AtualizarProdutoHandler : IRequestHandler<AtualizarProdutoQuery, IEvent>
    {
        private readonly IProdutoRepository _repository;
        public AtualizarProdutoHandler(IProdutoRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEvent> Handle(AtualizarProdutoQuery request, CancellationToken cancellationToken)
        {
            string mensagem = "";
            var success = false;
            try
            {
                var produto = new Produto();
                produto = await _repository.ObterPorId(request.Id, cancellationToken);
                if (produto != null)
                {
                    produto.Titulo = request.Titulo;
                    produto.Status = request.Status;
                    var result = await _repository.Salvar(produto, cancellationToken);
                    success = result.ModifiedCount > 0;
                    mensagem = success ? "Produto atualizado com sucesso!" : "Nenhuma atualização realizada.";
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
