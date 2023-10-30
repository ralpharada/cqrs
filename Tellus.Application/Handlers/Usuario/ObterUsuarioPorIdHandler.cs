
using Tellus.Application.Core;
using Tellus.Application.Mapper;
using Tellus.Application.Queries;
using Tellus.Application.Responses;
using Tellus.Core.Events;
using Tellus.Domain.Interfaces;
using MediatR;

namespace Tellus.Application.Handlers
{
    public class ObterUsuarioPorIdHandler : IRequestHandler<ObterUsuarioPorIdQuery, IEvent>
    {
        private readonly IUsuarioRepository _repository;
        private readonly ITipoDocumentoRepository _repositoryTipoDocumento;
        private readonly IPermissaoRepository _repositoryPermissao;
        private readonly IClienteRepository _repositoryCliente;
        private readonly UsuarioAutenticado _usuarioAutenticado;
        private readonly IProdutoRepository _repositoryProduto;

        public ObterUsuarioPorIdHandler(IUsuarioRepository repository, UsuarioAutenticado usuarioAutenticado, IClienteRepository repositoryCliente, ITipoDocumentoRepository repositoryTipoDocumento, IPermissaoRepository repositoryPermissao, IProdutoRepository repositoryProduto)
        {
            _repository = repository;
            _repositoryTipoDocumento = repositoryTipoDocumento;
            _repositoryPermissao = repositoryPermissao;
            _repositoryCliente = repositoryCliente;
            _usuarioAutenticado = usuarioAutenticado;
            _repositoryProduto = repositoryProduto;
        }

        public async Task<IEvent> Handle(ObterUsuarioPorIdQuery request, CancellationToken cancellationToken)
        {
            var success = false;
            var response = new UsuarioResponse();
            try
            {
                if (_usuarioAutenticado.Email == null)
                    return new ResultEvent(success, "Acesso expirado");

                var cliente = await _repositoryCliente.ObterPorEmailCadastroAtivo(_usuarioAutenticado.Email, cancellationToken);
                if (cliente != null)
                {
                    var taskResult = await _repository.ObterPorId(request.Id, cancellationToken);
                    if (taskResult != null)
                    {
                        if (taskResult.TipoDocumentoIds != null)
                            taskResult.TipoDocumentos = _repositoryTipoDocumento.ObterTodosPorIds(taskResult.TipoDocumentoIds, cliente.Id);
                        if (taskResult.PermissaoIds != null)
                            taskResult.Permissoes = _repositoryPermissao.ObterTodosPorIds(taskResult.PermissaoIds);
                        if (taskResult.ProdutoIds != null)
                            taskResult.Produtos =await _repositoryProduto.ObterPorIds(taskResult.ProdutoIds, cancellationToken);

                        var vinculoPermissaoResponse = new List<VinculoPermissaoResponse>();
                        if (taskResult.VinculoPermissoes != null)
                        {
                            foreach (var item in taskResult.VinculoPermissoes.Where(x => taskResult.TipoDocumentoIds.Exists(id => id == x.TipoDocumentoId)))
                            {
                                var tipoDocumento = taskResult.TipoDocumentos.Where(x => x.Id == item.TipoDocumentoId).FirstOrDefault();
                                var vinculoPermissoes = _repositoryPermissao.ObterTodosPorIds(item.PermissaoIds);
                                if (tipoDocumento != null)
                                    vinculoPermissaoResponse.Add(new() { Vinculo = new() { Id = tipoDocumento.Id, Nome = tipoDocumento.Nome, Status = tipoDocumento.Status }, Permissoes = PermissaoMapper<List<PermissaoResponse>>.Map(vinculoPermissoes) });
                            }
                        }
                        response = UsuarioMapper<UsuarioResponse>.Map(taskResult);
                        response.VinculoPermissao = vinculoPermissaoResponse;
                        success = true;
                    }
                    else
                        return new ResultEvent(success, "Usuário não localizado");
                }
                else
                    return new ResultEvent(success, "Conta não localizada");
            }
            catch (Exception ex)
            {
                return new ResultEvent(success, ex.Message);
            }
            return new ResultEvent(success, response);
        }

    }
}
