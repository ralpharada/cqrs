using Tellus.Application.Core;
using Tellus.Application.Queries;
using Tellus.Core.Events;
using Tellus.Domain.Enums;
using Tellus.Domain.Interfaces;
using Tellus.Domain.Models;
using MediatR;
using Newtonsoft.Json;

namespace Tellus.Application.Handlers
{
    public class AdicionarIndiceHandler : IRequestHandler<AdicionarIndiceQuery, IEvent>
    {
        private readonly IIndiceRepository _repository;
        private readonly ITipoDocumentoRepository _repositoryTipoDocumento;
        private readonly IClienteRepository _repositoryCliente;
        private readonly UsuarioAutenticado _usuarioAutenticado;
        private readonly ILogClienteRepository _logClienteRepository;

        public AdicionarIndiceHandler(IIndiceRepository repository, ITipoDocumentoRepository repositoryTipoDocumento, UsuarioAutenticado usuarioAutenticado, IClienteRepository repositoryCliente, ILogClienteRepository logClienteRepository)
        {
            _repository = repository;
            _repositoryTipoDocumento = repositoryTipoDocumento;
            _repositoryCliente = repositoryCliente;
            _usuarioAutenticado = usuarioAutenticado;
            _logClienteRepository = logClienteRepository;
        }
        public async Task<IEvent> Handle(AdicionarIndiceQuery request, CancellationToken cancellationToken)
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
                        var indice = new Indice
                        {
                            Id = Guid.NewGuid(),
                            ClienteId = tipoDocumento.ClienteId,
                            DataCadastro = DateTime.UtcNow,
                            DataUltimaAlteracao = DateTime.UtcNow,
                            Nome = request.Nome,
                            Mascara = request.Mascara,
                            Tamanho = request.Tamanho,
                            Obrigatorio=request.Obrigatorio,
                            ETipoIndice = (ETipoIndice)Enum.Parse(typeof(ETipoIndice), request.TipoIndice),
                            Lista = request.Lista
                        };
                        var resultIndice = await _repository.Salvar(indice, cancellationToken);
                        success = resultIndice.UpsertedId > 0;
                        if (success)
                        {
                            if (tipoDocumento.IndiceIds == null)
                                tipoDocumento.IndiceIds = new List<Guid>();
                            tipoDocumento.IndiceIds.Add(indice.Id);
                            if (tipoDocumento.Posicao == null)
                                tipoDocumento.Posicao = new List<Posicao>();
                            tipoDocumento.Posicao.Add(new Posicao() { Id = indice.Id, Valor = 99999 });
                            var resultTipoDocumento = await _repositoryTipoDocumento.Salvar(tipoDocumento, cancellationToken);
                            success = resultTipoDocumento.ModifiedCount > 0;
                            if (success)
                            {
                                mensagem = "Índice cadastrado com sucesso!";
                                await _logClienteRepository.Salvar(new LogCliente() { Id = Guid.NewGuid(), Pagina = "Criação do Indice", ClienteId = cliente.Id, Acao = JsonConvert.SerializeObject(indice), DataRegistro = DateTime.UtcNow }, cancellationToken);
                            }
                        }
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
