using Tellus.Application.Core;
using Tellus.Application.Mapper;
using Tellus.Application.Queries;
using Tellus.Application.Responses;
using Tellus.Core.Events;
using Tellus.Domain.Interfaces;
using MediatR;

namespace Tellus.Application.Handlers
{
    public class ObterGrupoUsuarioPorIdHandler : IRequestHandler<ObterGrupoUsuarioPorIdQuery, IEvent>
    {
        private readonly IGrupoUsuarioRepository _repository;
        private readonly ITipoDocumentoRepository _repositoryTipoDocumento;
        private readonly IUsuarioRepository _repositoryUsuario;
        private readonly IClienteRepository _repositoryCliente;
        private readonly UsuarioAutenticado _usuarioAutenticado;
        private readonly IPermissaoRepository _repositoryPermissao;
        private readonly IProdutoRepository _repositoryProduto;

        public ObterGrupoUsuarioPorIdHandler(IGrupoUsuarioRepository repository, IUsuarioRepository repositoryUsuario, ITipoDocumentoRepository repositoryTipoDocumento, UsuarioAutenticado usuarioAutenticado, IClienteRepository repositoryCliente, IPermissaoRepository repositoryPermissao, IProdutoRepository repositoryProduto)
        {
            _repository = repository;
            _repositoryUsuario = repositoryUsuario;
            _repositoryTipoDocumento = repositoryTipoDocumento;
            _repositoryCliente = repositoryCliente;
            _usuarioAutenticado = usuarioAutenticado;
            _repositoryPermissao = repositoryPermissao;
            _repositoryProduto = repositoryProduto;
        }
        public async Task<IEvent> Handle(ObterGrupoUsuarioPorIdQuery request, CancellationToken cancellationToken)
        {
            bool success = false;
            var response = new GrupoUsuarioResponse();
            try
            {
                if (_usuarioAutenticado.Email == null)
                    return new ResultEvent(success, "Acesso expirado");

                var cliente = await _repositoryCliente.ObterPorEmailCadastroAtivo(_usuarioAutenticado.Email, cancellationToken);
                if (cliente != null)
                {
                    var grupoUsuario = await _repository.ObterPorId(request.Id, cliente.Id, cancellationToken);
                    if (grupoUsuario.TipoDocumentoIds != null)
                        grupoUsuario.TipoDocumentos = _repositoryTipoDocumento.ObterTodosPorIds(grupoUsuario.TipoDocumentoIds, cliente.Id);
                    if (grupoUsuario.UsuarioIds != null)
                        grupoUsuario.Usuarios = _repositoryUsuario.ObterTodosPorIds(grupoUsuario.UsuarioIds, cliente.Id);
                    if (grupoUsuario.PermissaoIds != null)
                        grupoUsuario.Permissoes = _repositoryPermissao.ObterTodosPorIds(grupoUsuario.PermissaoIds);
                    if (grupoUsuario.ProdutoIds != null)
                        grupoUsuario.Produtos = await _repositoryProduto.ObterPorIds(grupoUsuario.ProdutoIds, cancellationToken);
                    var vinculoPermissaoResponse = new List<VinculoPermissaoResponse>();
                    if (grupoUsuario.VinculoPermissoes != null)
                    {
                        foreach (var item in grupoUsuario.VinculoPermissoes)
                        {
                            var tipoDocumento = grupoUsuario.TipoDocumentos.Where(x => x.Id == item.TipoDocumentoId).FirstOrDefault();
                            var vinculoPermissoes = _repositoryPermissao.ObterTodosPorIds(item.PermissaoIds);
                            if (tipoDocumento != null)
                                vinculoPermissaoResponse.Add(new() { Vinculo = new() { Id = tipoDocumento.Id, Nome = tipoDocumento.Nome, Status = tipoDocumento.Status }, Permissoes = PermissaoMapper<List<PermissaoResponse>>.Map(vinculoPermissoes) });
                        }
                    }
                    response = GrupoUsuarioMapper<GrupoUsuarioResponse>.Map(grupoUsuario);
                    response.VinculoPermissao = vinculoPermissaoResponse;
                    success = true;
                }
            }
            catch (Exception ex)
            {
                return new ResultEvent(success, ex.Message);
            }
            return new ResultEvent(success, response);
        }

    }
}
