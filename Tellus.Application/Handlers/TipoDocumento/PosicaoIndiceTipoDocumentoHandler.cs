using Tellus.Application.Core;
using Tellus.Application.Queries;
using Tellus.Core.Events;
using Tellus.Domain.Interfaces;
using Tellus.Domain.Models;
using MediatR;

namespace Tellus.Application.Handlers
{
    public class PosicaoIndiceTipoDocumentoHandler : IRequestHandler<PosicaoIndiceTipoDocumentoQuery, IEvent>
    {
        private readonly ITipoDocumentoRepository _repositoryTipoDocumento;
        private readonly IClienteRepository _repositoryCliente;
        private readonly UsuarioAutenticado _usuarioAutenticado;
        private readonly ILogClienteRepository _logClienteRepository;


        public PosicaoIndiceTipoDocumentoHandler(ITipoDocumentoRepository repositoryTipoDocumento, UsuarioAutenticado usuarioAutenticado, IClienteRepository repositoryCliente, ILogClienteRepository logClienteRepository)
        {
            _repositoryTipoDocumento = repositoryTipoDocumento;
            _repositoryCliente = repositoryCliente;
            _usuarioAutenticado = usuarioAutenticado;
            _logClienteRepository = logClienteRepository;
        }
        public async Task<IEvent> Handle(PosicaoIndiceTipoDocumentoQuery request, CancellationToken cancellationToken)
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
                    var tipoDocumento = await _repositoryTipoDocumento.ObterPorId(request.TipoDocumentoId, cliente.Id, cancellationToken);
                    if (tipoDocumento != null)
                    {
                        tipoDocumento.Posicao = new List<Posicao>();
                        int index = 0;
                        foreach (var item in request.IndiceIds)
                        {
                            tipoDocumento.Posicao.Add(new Posicao() { Id = item, Valor = index });
                            index++;
                        }

                        var resultTipoDocumento = await _repositoryTipoDocumento.Salvar(tipoDocumento, cancellationToken);
                        success = resultTipoDocumento.ModifiedCount > 0;
                        if (success)
                            await _logClienteRepository.Salvar(new LogCliente() { Id = Guid.NewGuid(), Pagina = "Tipo Documento x Índices", ClienteId = cliente.Id, Acao = "Ordenação dos campos do tipo de documento " + tipoDocumento.Nome, DataRegistro = DateTime.UtcNow }, cancellationToken);
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
