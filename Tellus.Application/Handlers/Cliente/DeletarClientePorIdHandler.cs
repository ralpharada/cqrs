using Tellus.Application.Queries;
using Tellus.Core.Events;
using Tellus.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Configuration;
using Tellus.Application.Core;

namespace Tellus.Application.Handlers
{
    public class DeletarClientePorIdHandler : IRequestHandler<DeletarClientePorIdQuery, IEvent>
    {
        private readonly UsuarioAutenticado _usuarioAutenticado;
        private readonly IClienteRepository _repositoryCliente;
        private readonly IUsuarioAdmRepository _repository;
        private readonly IConfiguration _configuration;
        private readonly IAmazonRepository _repositoryAmazon;

        public DeletarClientePorIdHandler(IClienteRepository repositoryCliente, UsuarioAutenticado usuarioAutenticado, IUsuarioAdmRepository repository, IAmazonRepository repositoryAmazon, IConfiguration configuration)
        {
            _repositoryCliente = repositoryCliente;
            _repository = repository;
            _usuarioAutenticado = usuarioAutenticado;
            _configuration = configuration;
            _repositoryAmazon = repositoryAmazon;
        }

        public async Task<IEvent> Handle(DeletarClientePorIdQuery request, CancellationToken cancellationToken)
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
                    _repositoryAmazon.DeleteFolder(_configuration.GetSection("AmazonS2:ACCESS_KEY_ID").Value, _configuration.GetSection("AmazonS2:SECRET_ACCESS_KEY").Value, _configuration.GetSection("AmazonS2:Bucket").Value, request.Id.ToString());
                    success = await _repository.DeletarPorId(request.Id, cancellationToken);
                    mensagem = success ? "Cliente excluído com sucesso!" : "Não foi possível excluir o Cliente.";
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
