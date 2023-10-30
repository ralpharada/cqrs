using Tellus.Application.Core;
using Tellus.Application.Crypto;
using Tellus.Application.Queries;
using Tellus.Application.Util;
using Tellus.Core.Events;
using Tellus.Domain.Interfaces;
using Tellus.Domain.Models;
using MediatR;

namespace Tellus.Application.Handlers
{
    public class AtualizarSenhaClienteLogadoHandler : IRequestHandler<AtualizarSenhaClienteLogadoQuery, IEvent>
    {
        private readonly IClienteRepository _repository;
        private readonly UsuarioAutenticado _usuarioAutenticado;
        private readonly EnviarEmail _enviarEmail;
        private readonly ILogClienteRepository _logClienteRepository;
        public AtualizarSenhaClienteLogadoHandler(IClienteRepository repository, UsuarioAutenticado usuarioAutenticado, EnviarEmail enviarEmail, ILogClienteRepository logClienteRepository)
        {
            _repository = repository;
            _usuarioAutenticado = usuarioAutenticado;
            _enviarEmail = enviarEmail;
            _logClienteRepository = logClienteRepository;
        }

        public async Task<IEvent> Handle(AtualizarSenhaClienteLogadoQuery request, CancellationToken cancellationToken)
        {
            string mensagem = "";
            var success = false;
            try
            {
                if (_usuarioAutenticado.Email == null)
                    return new ResultEvent(success, "Acesso expirado");
                var cliente = await _repository.ObterPorEmailCadastroAtivo(_usuarioAutenticado.Email, cancellationToken);
                if (cliente != null)
                {
                    if (Criptografia.Verify(request.SenhaAtual, cliente.Senha))
                    {
                        cliente.Senha = Criptografia.Encrypt(request.NovaSenha);
                        var result = await _repository.Salvar(cliente, cancellationToken);
                        success = result.ModifiedCount > 0;
                        if (success)
                        {
                            await _logClienteRepository.Salvar(new LogCliente() { Id = Guid.NewGuid(), Pagina = "Atualização de senha", ClienteId = cliente.Id, Acao = "", DataRegistro = DateTime.UtcNow }, cancellationToken);
                       //     _enviarEmail.Send(cliente.Responsaveis.FirstOrDefault(x => x.TipoResponsavel == ETipoResponsavel.Contratacao).Email, "Atualização de senha", "Sua senha de acesso ao sistema foi atualizado.", null);
                            mensagem = "Atualizado com sucesso!";
                        }
                    }
                    else
                    {
                        mensagem = "Erro ao atualizar a senha. Senha atual inválida!";
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
