using Tellus.Application.Core;
using Tellus.Application.Mapper;
using Tellus.Application.Queries;
using Tellus.Application.Responses;
using Tellus.Core.Events;
using Tellus.Domain.Interfaces;
using Tellus.Domain.Models;
using MediatR;
using Microsoft.Extensions.Configuration;

namespace Tellus.Application.Handlers
{
    public class ObterTodosTipoDocumentoCompletoPorUsuarioLogadoHandler : IRequestHandler<ObterTodosTipoDocumentoCompletoPorUsuarioLogadoQuery, IEvent>
    {
        private readonly ITipoDocumentoRepository _repository;
        private readonly IConfiguration _configuration;
        private readonly IIndiceRepository _repositoryIndice;
        private readonly IUsuarioRepository _repositoryUsuario;
        private readonly UsuarioAutenticado _usuarioAutenticado;
        public ObterTodosTipoDocumentoCompletoPorUsuarioLogadoHandler(ITipoDocumentoRepository repository, IIndiceRepository repositoryIndice, IConfiguration configuration, UsuarioAutenticado usuarioAutenticado, IUsuarioRepository repositoryUsuario)
        {
            _repository = repository;
            _repositoryIndice = repositoryIndice;
            _configuration = configuration;
            _repositoryUsuario = repositoryUsuario;
            _usuarioAutenticado = usuarioAutenticado;
        }
        public async Task<IEvent> Handle(ObterTodosTipoDocumentoCompletoPorUsuarioLogadoQuery request, CancellationToken cancellationToken)
        {
            bool success = false;
            var response = new List<TipoDocumentoResponse>();
            long total = 0;
            try
            {
                if (_usuarioAutenticado.Email == null)
                    return new ResultEvent(success, "Acesso expirado");

                var usuario = await _repositoryUsuario.ObterPorEmailCadastroAtivo(_usuarioAutenticado.Email, cancellationToken);
                if (usuario != null)
                {
                    var taskResult = await _repository.ObterTodosCompleto(usuario.ClienteId, cancellationToken);
                    foreach (var tipoDocumento in taskResult)
                    {
                        tipoDocumento.Indices = new List<Indice>();
                        if (tipoDocumento.IndiceIds != null)
                            tipoDocumento.Indices = _repositoryIndice.ObterTodosPorIds(tipoDocumento.IndiceIds, usuario.ClienteId);
                        if (tipoDocumento.Posicao == null) tipoDocumento.Posicao = new List<Posicao>();
                        tipoDocumento.Indices.ForEach(x =>
                        {
                            var indice = tipoDocumento.Posicao.Find(p => p.Id == x.Id);
                            x.Ordem = (indice != null) ? indice.Valor : 99999;
                        });
                    }
                    response = TipoDocumentoMapper<List<TipoDocumentoResponse>>.Map(taskResult);
                    success = true;
                }
            }
            catch (Exception ex)
            {
                return new ResultEvent(success, ex.Message);
            }
            return new ResultEvent(success, response, total);
        }

    }
}
