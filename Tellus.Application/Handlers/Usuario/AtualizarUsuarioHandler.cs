using Tellus.Application.Core;
using Tellus.Application.Crypto;
using Tellus.Application.Queries;
using Tellus.Core.Events;
using Tellus.Domain.Interfaces;
using Tellus.Domain.Models;
using MediatR;
using Newtonsoft.Json;

namespace Tellus.Application.Handlers
{
    public class AtualizarUsuarioHandler : IRequestHandler<AtualizarUsuarioQuery, IEvent>
    {
        private readonly IUsuarioRepository _repository;
        private readonly IClienteRepository _repositoryCliente;
        private readonly UsuarioAutenticado _usuarioAutenticado;
        private readonly ILogClienteRepository _logClienteRepository;
        public AtualizarUsuarioHandler(IUsuarioRepository repository, UsuarioAutenticado usuarioAutenticado, IClienteRepository repositoryCliente, ILogClienteRepository logClienteRepository)
        {
            _repository = repository;
            _repositoryCliente = repositoryCliente;
            _usuarioAutenticado = usuarioAutenticado;
            _logClienteRepository = logClienteRepository;
        }

        public async Task<IEvent> Handle(AtualizarUsuarioQuery request, CancellationToken cancellationToken)
        {
            string mensagem = "";
            var success = false;
            try
            {
                var cliente = await _repositoryCliente.ObterPorEmailCadastroAtivo(_usuarioAutenticado.Email, cancellationToken);
                if (cliente != null)
                {
                    var usuario = new Usuario();
                    usuario = await _repository.ObterPorId(request.Id, cancellationToken);
                    if (usuario != null && usuario.ClienteId == cliente.Id)
                    {
                        usuario.Nome = request.Nome;
                        usuario.Email = request.Email;
                        usuario.Status = request.Status;
                        if(!string.IsNullOrEmpty(request.Senha))
                        usuario.Senha = Criptografia.Encrypt(request.Senha);
                        var result = await _repository.Salvar(usuario, cancellationToken);
                        success = result.ModifiedCount > 0;
                        mensagem = success ? "Usuário atualizado com sucesso!" : "Nenhuma atualização realizada.";
                        if(success)
                            await _logClienteRepository.Salvar(new LogCliente() { Id = Guid.NewGuid(), Pagina = "Atualização de Usuário", ClienteId = cliente.Id, Acao = JsonConvert.SerializeObject(usuario), DataRegistro = DateTime.UtcNow }, cancellationToken);
                    }
                    else
                    {
                        mensagem = "Não foi possível atualizar o usuário.";
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
