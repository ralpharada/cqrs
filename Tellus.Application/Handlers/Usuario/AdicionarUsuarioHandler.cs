using Tellus.Application.Core;
using Tellus.Application.Crypto;
using Tellus.Application.Mapper;
using Tellus.Application.Queries;
using Tellus.Application.Util;
using Tellus.Core.Events;
using Tellus.Domain.Interfaces;
using Tellus.Domain.Models;
using MediatR;
using Newtonsoft.Json;

namespace Tellus.Application.Handlers
{
    public class AdicionarUsuarioHandler : IRequestHandler<AdicionarUsuarioQuery, IEvent>
    {
        private readonly IUsuarioRepository _repository;
        private readonly EnviarEmail _enviarEmail;
        private readonly IClienteRepository _repositoryCliente;
        private readonly UsuarioAutenticado _usuarioAutenticado;
        private readonly ILogClienteRepository _logClienteRepository;

        public AdicionarUsuarioHandler(IUsuarioRepository repository, EnviarEmail enviarEmail, UsuarioAutenticado usuarioAutenticado, IClienteRepository repositoryCliente,ILogClienteRepository logClienteRepository)
        {
            _repository = repository;
            _enviarEmail = enviarEmail;
            _repositoryCliente = repositoryCliente;
            _usuarioAutenticado = usuarioAutenticado;
            _logClienteRepository = logClienteRepository;
        }

        public async Task<IEvent> Handle(AdicionarUsuarioQuery request, CancellationToken cancellationToken)
        {
            string mensagem = "";
            var success = false;
            Guid id = Guid.Empty;
            try
            {
                if (_usuarioAutenticado.Email == null)
                    return new ResultEvent(success, "Acesso expirado");

                var cliente = await _repositoryCliente.ObterPorEmailCadastroAtivo(_usuarioAutenticado.Email, cancellationToken);
                if (cliente != null)
                {
                    var existsUser = await _repository.ExisteUsuario(request.Email, cancellationToken);
                    if (existsUser)
                    {
                        mensagem = "Jà existe um usuário com esse e-mail";

                    }
                    var usuario = UsuarioMapper<Usuario>.Map(request);
                    var hash = Guid.NewGuid().ToString("N");
                    usuario.Id = Guid.NewGuid();
                    usuario.ClienteId = cliente.Id;
                    usuario.Senha = Criptografia.Encrypt(request.Senha);
                    usuario.HashEsqueciSenha = String.Empty;
                    usuario.DataCadastro = DateTime.UtcNow;
                    var result = await _repository.Salvar(usuario, cancellationToken);
                    success = result.UpsertedId > 0;
                    if (success)
                    {
                        await _logClienteRepository.Salvar(new LogCliente() { Id = Guid.NewGuid(), Pagina = "Novo Usuário", ClienteId = cliente.Id, Acao = JsonConvert.SerializeObject(usuario), DataRegistro = DateTime.UtcNow }, cancellationToken);
                        id = ((Guid)result.UpsertedId);
                        mensagem = "Cadastro efetuado com sucesso!";
                    //    _enviarEmail.Send(usuario.Email, "Seja bem vindo!", "Para ter acesso ao sistema, clique no link abaixo:<br/> <a href=\"https://localhost:5001/portal/api/validarCadastroUsuarioViaLink/" + hash + "\">https://localhost:5001/portal/api/validarCadastroUsuarioViaLink/" + hash + "</a>", null);
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
