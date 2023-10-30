using Tellus.Application.Crypto;
using Tellus.Application.Queries;
using Tellus.Core.Events;
using Tellus.Domain.Interfaces;
using Tellus.Domain.Models;
using MediatR;
using Tellus.Application.Core;

namespace Tellus.Application.Handlers
{
    public class AtualizarClienteHandler : IRequestHandler<AtualizarClienteQuery, IEvent>
    {
        private readonly UsuarioAutenticado _usuarioAutenticado;
        private readonly IClienteRepository _repositoryCliente;
        private readonly IUsuarioAdmRepository _repository;
        public AtualizarClienteHandler(IClienteRepository repositoryCliente, UsuarioAutenticado usuarioAutenticado, IUsuarioAdmRepository repository)
        {

            _repositoryCliente = repositoryCliente;
            _repository = repository;
            _usuarioAutenticado = usuarioAutenticado;
        }

        public async Task<IEvent> Handle(AtualizarClienteQuery request, CancellationToken cancellationToken)
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
                    var cliente = new Cliente();
                    cliente = await _repositoryCliente.ObterPorId(request.Id, cancellationToken);
                    if (cliente != null)
                    {
                        cliente.Documento = request.Documento;
                        cliente.Nome = request.Nome;
                        cliente.Email = request.Email;
                        cliente.Endereco = request.Endereco;
                        cliente.QtdeUsuarios = request.QtdeUsuarios;
                        cliente.EspacoDisco = request.EspacoDisco;
                        cliente.Status = request.Status;
                        if (!String.IsNullOrEmpty(request.Senha))
                            cliente.Senha = Criptografia.Encrypt(request.Senha);
                        var result = await _repositoryCliente.Salvar(cliente, cancellationToken);
                        success = result.ModifiedCount > 0;
                        mensagem = success ? "Cliente atualizado com sucesso!" : "Nenhuma atualização realizada.";
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
