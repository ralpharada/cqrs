using Tellus.Application.Core;
using Tellus.Application.Mapper;
using Tellus.Application.Queries;
using Tellus.Application.Responses;
using Tellus.Core.Events;
using Tellus.Domain.Interfaces;
using MediatR;
using Tellus.Domain.Models;

namespace Tellus.Application.Handlers
{
    public class ObterTodosIndicePorTipoDocumentoIdHandler : IRequestHandler<ObterTodosIndicePorTipoDocumentoIdQuery, IEvent>
    {
        private readonly IIndiceRepository _repository;
        private readonly ITipoDocumentoRepository _repositoryTipoDocumento;
        private readonly IClienteRepository _repositoryCliente;
        private readonly UsuarioAutenticado _usuarioAutenticado;

        public ObterTodosIndicePorTipoDocumentoIdHandler(IIndiceRepository repository, ITipoDocumentoRepository repositoryTipoDocumento, UsuarioAutenticado usuarioAutenticado, IClienteRepository repositoryCliente)
        {
            _repository = repository;
            _repositoryTipoDocumento = repositoryTipoDocumento;
            _repositoryCliente = repositoryCliente;
            _usuarioAutenticado = usuarioAutenticado;
        }
        public async Task<IEvent> Handle(ObterTodosIndicePorTipoDocumentoIdQuery request, CancellationToken cancellationToken)
        {
            bool success = false;
            var response = new List<IndiceResponse>();
            try
            {
                var cliente = await _repositoryCliente.ObterPorEmailCadastroAtivo(_usuarioAutenticado.Email, cancellationToken);
                if (cliente != null)
                {
                    var tipoDocumento = await _repositoryTipoDocumento.ObterPorId(request.TipoDocumentoId, cliente.Id, cancellationToken);
                    if (tipoDocumento != null)
                    {
                        if (tipoDocumento.IndiceIds != null)
                        {
                            if (tipoDocumento.Posicao == null) tipoDocumento.Posicao = new List<Posicao>();
                            var lista = _repository.ObterTodosPorIds(tipoDocumento.IndiceIds, cliente.Id);
                            lista.ForEach(x =>
                            {
                                var indice = tipoDocumento.Posicao.Find(p => p.Id == x.Id);
                                x.Ordem = (indice != null) ? indice.Valor : 99999;
                            });
                            response = IndiceMapper<List<IndiceResponse>>.Map(lista);
                        }
                        success = true;
                    }
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
