using Tellus.Application.Core;
using Tellus.Application.Queries;
using Tellus.Core.Events;
using Tellus.Domain.Enums;
using Tellus.Domain.Interfaces;
using Tellus.Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Tellus.Application.Handlers
{
    public class AtualizarIndiceValorHandler : IRequestHandler<AtualizarIndiceValorQuery, IEvent>
    {
        private readonly IDocumentoRepository _repository;
        private readonly ILogDocumentoRepository _repositoryLogDocumento;
        private readonly IUsuarioRepository _repositoryUsuario;
        private readonly IIndiceRepository _repositoryIndice;
        private readonly UsuarioAutenticado _usuarioAutenticado;
        private readonly IClienteRepository _repositoryCliente;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMediator _mediator;

        public AtualizarIndiceValorHandler(IDocumentoRepository repository,
            ILogDocumentoRepository repositoryLogDocumento,
            IIndiceRepository repositoryIndice,
            UsuarioAutenticado usuarioAutenticado,
            IUsuarioRepository repositoryUsuario,
            IClienteRepository repositoryCliente,
        IHttpContextAccessor httpContextAccessor,
        IMediator mediator)
        {
            _repository = repository;
            _repositoryLogDocumento = repositoryLogDocumento;
            _repositoryUsuario = repositoryUsuario;
            _repositoryIndice = repositoryIndice;
            _usuarioAutenticado = usuarioAutenticado;
            _repositoryCliente = repositoryCliente;
            _httpContextAccessor = httpContextAccessor;
            _mediator = mediator;
        }
        public async Task<IEvent> Handle(AtualizarIndiceValorQuery request, CancellationToken cancellationToken)
        {
            string mensagem = "";
            bool success = false;
            try
            {
                if (_usuarioAutenticado.Email == null)
                    return new ResultEvent(success, "Acesso expirado");

                var usuario = await _repositoryUsuario.ObterPorEmailCadastroAtivo(_usuarioAutenticado.Email, cancellationToken);
                if (usuario != null)
                {
                    var cliente = await _repositoryCliente.ObterPorId(usuario.ClienteId, cancellationToken);
                    if (cliente != null && cliente.Status)
                    {
                        var ip = _httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();
                        await _mediator.Send(new AtualizarUsuarioLogadoQuery(usuario.Id, ip), cancellationToken);

                        Documento documento = await _repository.ObterPorId(request.Id, usuario.ClienteId, cancellationToken);
                        if (documento != null)
                        {
                            List<Guid> idIndices = new List<Guid>();
                            documento.IndiceValores.ForEach(i => idIndices.Add(i.IndiceId));
                            List<Indice> indices = _repositoryIndice.ObterTodosPorIds(idIndices, usuario.ClienteId);
                            List<LogValor> indicesAtualizados = new List<LogValor>();
                            foreach (var item in documento.IndiceValores)
                            {
                                var indiceValor = request.IndiceValores.FirstOrDefault(x => x.IndiceId == item.IndiceId);
                                var indice = indices.FirstOrDefault(x => x.Id == item.IndiceId);
                                if (indiceValor != null)
                                {
                                    LogValor logValor = new LogValor();
                                    logValor.Nome = indice.Nome;
                                    switch (indice.ETipoIndice)
                                    {
                                        case ETipoIndice.Caractere:
                                            if (indiceValor.Texto != item.Texto)
                                            {
                                                logValor.Valor = indiceValor.Texto;
                                            }
                                            break;
                                        case ETipoIndice.Numero:
                                            if (indiceValor.Numero != item.Numero)
                                            {
                                                logValor.Valor = indiceValor.Numero.ToString();
                                            }
                                            break;
                                        case ETipoIndice.Data:
                                            if (item.Data.HasValue)
                                            {
                                                if (indiceValor.Data.Value.ToShortDateString() != item.Data.Value.ToShortDateString())
                                                    logValor.Valor = indiceValor.Data.Value.ToShortDateString();
                                            }
                                            else
                                                if (indiceValor.Data.HasValue)
                                                logValor.Valor = indiceValor.Data.Value.ToShortDateString();
                                            break;
                                        case ETipoIndice.Hora:
                                            if (indiceValor.Hora.HasValue)
                                            {
                                                if (indiceValor.Hora.ToString() != item.Hora.ToString())
                                                    logValor.Valor = indiceValor.Hora.Value.ToShortTimeString();
                                            }
                                            else
                                                if (indiceValor.Hora.HasValue)
                                                logValor.Valor = indiceValor.Hora.Value.ToShortTimeString();
                                            break;
                                        case ETipoIndice.Decimal:
                                            if (indiceValor.Decimal != item.Decimal)
                                            {
                                                logValor.Valor = indiceValor.Decimal.ToString();
                                            }
                                            break;
                                        case ETipoIndice.Lista:
                                            if (indiceValor.Texto != item.Texto)
                                            {
                                                logValor.Valor = indiceValor.Texto;
                                            }
                                            break;
                                        default:
                                            break;
                                    }
                                    if (logValor.Valor != null)
                                        indicesAtualizados.Add(logValor);
                                }
                            }
                            foreach (var indice in indices)
                            {
                                if (!request.IndiceValores.Exists(x => x.IndiceId == indice.Id))
                                {
                                    IndiceValor indiceValor = new IndiceValor();
                                    indiceValor.IndiceId = indice.Id;
                                    indiceValor.ETipoIndice = indice.ETipoIndice.ToString();
                                    switch (indice.ETipoIndice)
                                    {
                                        case ETipoIndice.Numero:
                                            indiceValor.Numero = null;
                                            break;
                                        case ETipoIndice.Booleano:
                                            break;
                                        case ETipoIndice.Data:
                                            indiceValor.Data = null;
                                            break;
                                        case ETipoIndice.Hora:
                                            indiceValor.Hora = null;
                                            break;
                                        case ETipoIndice.Decimal:
                                            indiceValor.Decimal = null;
                                            break;
                                        case ETipoIndice.Lista:
                                            break;
                                        default:
                                            indiceValor.Texto = null;
                                            break;
                                    }
                                    request.IndiceValores.Add(indiceValor);
                                }
                            }
                            request.IndiceValores.ForEach(x =>
                            {
                                switch (Enum.Parse(typeof(ETipoIndice), x.ETipoIndice))
                                {
                                    case ETipoIndice.Numero:
                                        if (x.Numero.HasValue)
                                            x.Texto = x.Numero.ToString();
                                        break;
                                    case ETipoIndice.Booleano:
                                        break;
                                    case ETipoIndice.Data:
                                        if (x.Data.HasValue)
                                            x.Texto = x.Data.Value.ToShortDateString();
                                        break;
                                    case ETipoIndice.Hora:
                                        if (x.Hora.HasValue)
                                            x.Texto = x.Hora.Value.ToShortTimeString();
                                        break;
                                    case ETipoIndice.Decimal:
                                        if (x.Decimal.HasValue)
                                            x.Texto = x.Decimal.ToString();
                                        break;
                                    case ETipoIndice.Lista:
                                        break;
                                    default:
                                        break;
                                }
                            });
                            documento.IndiceValores = request.IndiceValores;
                            documento.DataUltimaAlteracao = DateTime.UtcNow;
                            var result = await _repository.Salvar(documento, cancellationToken);
                            success = result.ModifiedCount > 0;
                            if (success)
                            {
                                if (indicesAtualizados.Count > 0)
                                {
                                    LogDocumento logDocumento = new()
                                    {
                                        Id = Guid.NewGuid(),
                                        ClienteId = usuario.ClienteId,
                                        DocumentoId = documento.Id,
                                        DataRegistro = DateTime.UtcNow,
                                        Acao = "Edição do Documento",
                                        Valores = indicesAtualizados
                                    };

                                    await _repositoryLogDocumento.Salvar(logDocumento, cancellationToken);
                                }
                            }
                        }
                        mensagem = success ? "Documento atualizado com sucesso!" : "Nenhuma atualização realizada.";
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
