using Tellus.Application.Core;
using Tellus.Application.Queries;
using Tellus.Core.Events;
using Tellus.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Configuration;

namespace Tellus.Application.Handlers
{
    public class DeletarDocumentoHandler : IRequestHandler<DeletarDocumentoPorIdQuery, IEvent>
    {
        private readonly IDocumentoRepository _repository;
        private readonly IUsuarioRepository _repositoryUsuario;
        private readonly UsuarioAutenticado _usuarioAutenticado;
        private readonly IClienteRepository _repositoryCliente;
        private readonly IAmazonRepository _repositoryAmazon;
        private readonly IConfiguration _configuration;

        public DeletarDocumentoHandler(IDocumentoRepository repository, UsuarioAutenticado usuarioAutenticado, IUsuarioRepository repositoryUsuario, IClienteRepository repositoryCliente, IAmazonRepository repositoryAmazon, IConfiguration configuration)
        {
            _repository = repository;
            _repositoryUsuario = repositoryUsuario;
            _usuarioAutenticado = usuarioAutenticado;
            _repositoryCliente = repositoryCliente;
            _repositoryAmazon = repositoryAmazon;
            _configuration = configuration;
        }
        public async Task<IEvent> Handle(DeletarDocumentoPorIdQuery request, CancellationToken cancellationToken)
        {
            string mensagem = "";
            bool success = false;
            try
            {
                if (_usuarioAutenticado.Email == null)
                    return new ResultEvent(success, "Acesso expirado");

                var usuario = await _repositoryUsuario.ObterPorEmailCadastroAtivo(_usuarioAutenticado.Email, cancellationToken);
                if (usuario != null)
                {
                    var cliente = await _repositoryCliente.ObterPorId(usuario.ClienteId, cancellationToken);
                    if (cliente != null && cliente.Status)
                    {
                        var documento = await _repository.ObterPorId(request.Id, usuario.ClienteId, cancellationToken);
                       // var uploadOK = await _repositoryAmazon.Delete(_configuration.GetSection("AmazonS2:ACCESS_KEY_ID").Value, _configuration.GetSection("AmazonS2:SECRET_ACCESS_KEY").Value, _configuration.GetSection("AmazonS2:Bucket").Value, cliente.Id.ToString(), documento.Id.ToString());
                        success = await _repository.DeletarPorId(request.Id, usuario.ClienteId, cancellationToken);
                        mensagem = success ? "Documento excluído com sucesso!" : "Não foi possível excluir o Documento.";
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
