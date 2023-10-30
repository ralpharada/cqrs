using Tellus.Application.Crypto;
using Tellus.Application.Mapper;
using Tellus.Application.Queries;
using Tellus.Application.Util;
using Tellus.Core.Events;
using Tellus.Domain.Interfaces;
using Tellus.Domain.Models;
using MediatR;
using Microsoft.Extensions.Configuration;

namespace Tellus.Application.Handlers
{
    public class AdicionarProdutoHandler : IRequestHandler<AdicionarProdutoQuery, IEvent>
    {
        private readonly IProdutoRepository _repository;
        private readonly EnviarEmail _enviarEmail;
        private readonly IConfiguration _configuration;

        public AdicionarProdutoHandler(IProdutoRepository repository, EnviarEmail enviarEmail, IConfiguration configuration)
        {
            _repository = repository;
            _enviarEmail = enviarEmail;
            _configuration = configuration;
        }

        public async Task<IEvent> Handle(AdicionarProdutoQuery request, CancellationToken cancellationToken)
        {
            string mensagem = "";
            var success = false;
            try
            {
                var produto = new Produto();
                var hash = Guid.NewGuid().ToString("N");
                produto.Id = Guid.NewGuid();
                produto.Titulo = request.Titulo;
                produto.Status = true;
                var result = await _repository.Salvar(produto, cancellationToken);
                success = result.UpsertedId > 0;
                mensagem = success ? "Produto cadastrado com sucesso!" : "Falha ao cadastrar o produto";
            }
            catch (Exception ex)
            {
                mensagem = "Falha ao cadastrar o produto.";
            }
            return new ResultEvent(success, mensagem);
        }

    }
}
