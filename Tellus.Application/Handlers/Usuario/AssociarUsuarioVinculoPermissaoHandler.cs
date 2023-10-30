using Tellus.Application.Core;
using Tellus.Application.Queries;
using Tellus.Core.Events;
using Tellus.Domain.Interfaces;
using Tellus.Domain.Models;
using MediatR;
using Newtonsoft.Json;

namespace Tellus.Application.Handlers
{
    public class AssociarUsuarioVinculoPermissaoHandler : IRequestHandler<AssociarUsuarioVinculoPermissaoQuery, IEvent>
    {
        private readonly UsuarioAutenticado _usuarioAutenticado;
        private readonly IClienteRepository _repositoryCliente;
        private readonly IUsuarioRepository _repository;
        private readonly ILogClienteRepository _logClienteRepository;

        public AssociarUsuarioVinculoPermissaoHandler(IClienteRepository repositoryCliente, UsuarioAutenticado usuarioAutenticado, IUsuarioRepository repository, ILogClienteRepository logClienteRepository)
        {
            _repositoryCliente = repositoryCliente;
            _repository = repository;
            _usuarioAutenticado = usuarioAutenticado;
            _logClienteRepository = logClienteRepository;
        }
        public async Task<IEvent> Handle(AssociarUsuarioVinculoPermissaoQuery request, CancellationToken cancellationToken)
        {
            string mensagem = "";
            bool success = false;
            try
            {
                if (_usuarioAutenticado.Email == null)
                    return new ResultEvent(success, "Acesso expirado");

                var cliente = await _repositoryCliente.ObterPorEmailCadastroAtivo(_usuarioAutenticado.Email, cancellationToken);
                if (cliente != null)
                {
                    var usuario = await _repository.ObterPorId(request.UsuarioId, cancellationToken);
                    if (usuario != null)
                    {
                        usuario.VinculoPermissoes = request.VinculoPermissao;
                        var resultPermissao = await _repository.Salvar(usuario, cancellationToken);
                        success = resultPermissao.ModifiedCount > 0;
                        mensagem = success ? "Associação com sucesso!" : "Nenhuma atualização realizada.";
                        if (success)
                            await _logClienteRepository.Salvar(new LogCliente() { Id = Guid.NewGuid(), Pagina = "Associação do Usuário: Tipo Documento x Permissão", ClienteId = cliente.Id, Acao = "Usuário: " + usuario.Nome + "<br/>Permissão: <br/>" + JsonConvert.SerializeObject(request.VinculoPermissao), DataRegistro = DateTime.UtcNow }, cancellationToken);
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
