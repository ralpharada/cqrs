using Tellus.Application.Core;
using Tellus.Application.Queries;
using Tellus.Core.Events;
using Tellus.Domain.Interfaces;
using MediatR;
using Tellus.Domain.Models;
using Tellus.Infra.Repositories;
using System.Linq;

namespace Tellus.Application.Handlers
{
    public class AssociarGrupoUsuarioProdutoHandler : IRequestHandler<AssociarGrupoUsuarioProdutoQuery, IEvent>
    {
        private readonly IGrupoUsuarioRepository _repositoryGrupoUsuario;
        private readonly IClienteRepository _repositoryCliente;
        private readonly UsuarioAutenticado _usuarioAutenticado;
        public AssociarGrupoUsuarioProdutoHandler(IGrupoUsuarioRepository repositoryGrupoUsuario, UsuarioAutenticado usuarioAutenticado, IClienteRepository repositoryCliente)
        {
            _repositoryGrupoUsuario = repositoryGrupoUsuario;
            _repositoryCliente = repositoryCliente;
            _usuarioAutenticado = usuarioAutenticado;
        }
        public async Task<IEvent> Handle(AssociarGrupoUsuarioProdutoQuery request, CancellationToken cancellationToken)
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
                    var grupoUsuario = await _repositoryGrupoUsuario.ObterPorId(request.Id, cliente.Id, cancellationToken);
                    if (grupoUsuario != null)
                    {
                        grupoUsuario.ProdutoIds = request.ProdutoIds;
                       if(!grupoUsuario.ProdutoIds.Contains(Guid.Parse("76ddcef1-01a2-4a28-9397-3161c5d1d608")))
                        {
                            grupoUsuario.TipoDocumentoIds = new List<Guid>();
                            grupoUsuario.VinculoPermissoes = new List<VinculoPermissao>();
                        }
                        var resultTipoDocumento = await _repositoryGrupoUsuario.Salvar(grupoUsuario, cancellationToken);
                        success = resultTipoDocumento.ModifiedCount > 0;
                        mensagem = success ? "Associação com sucesso!" : "Nenhuma atualização realizada.";
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
