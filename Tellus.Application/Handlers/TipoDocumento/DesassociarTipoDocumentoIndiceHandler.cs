using Tellus.Application.Core;
using Tellus.Application.Queries;
using Tellus.Core.Events;
using Tellus.Domain.Interfaces;
using Tellus.Domain.Models;
using MediatR;
using Newtonsoft.Json;

namespace Tellus.Application.Handlers
{
	public class DesassociarTipoDocumentoIndiceHandler : IRequestHandler<DesassociarTipoDocumentoIndiceQuery, IEvent>
	{
		private readonly ITipoDocumentoRepository _repositoryTipoDocumento;
		private readonly IClienteRepository _repositoryCliente;
		private readonly UsuarioAutenticado _usuarioAutenticado;
		private readonly ILogClienteRepository _logClienteRepository;
		private readonly IIndiceRepository _indiceRepository;

		public DesassociarTipoDocumentoIndiceHandler(ITipoDocumentoRepository repositoryTipoDocumento, UsuarioAutenticado usuarioAutenticado, IClienteRepository repositoryCliente, ILogClienteRepository logClienteRepository, IIndiceRepository indiceRepository)
		{
			_repositoryTipoDocumento = repositoryTipoDocumento;
			_repositoryCliente = repositoryCliente;
			_usuarioAutenticado = usuarioAutenticado;
			_logClienteRepository = logClienteRepository;
			_indiceRepository = indiceRepository;
		}
		public async Task<IEvent> Handle(DesassociarTipoDocumentoIndiceQuery request, CancellationToken cancellationToken)
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
					var tipoDocumento = await _repositoryTipoDocumento.ObterPorId(request.Id, cliente.Id, cancellationToken);
					if (tipoDocumento != null)
					{
						tipoDocumento.IndiceIds.RemoveAll(x => x == request.IndiceId);
						var indices = _indiceRepository.ObterTodosPorIds(tipoDocumento.IndiceIds, cliente.Id);
						var resultTipoDocumento = await _repositoryTipoDocumento.Salvar(tipoDocumento, cancellationToken);
						success = resultTipoDocumento.ModifiedCount > 0;

						mensagem = success ? "Desassociação com sucesso!" : "Nenhuma atualização realizada.";
						if (success)
							await _logClienteRepository.Salvar(new LogCliente() { Id = Guid.NewGuid(), Pagina = "Desassociação do Tipo Documento x Indices", ClienteId = cliente.Id, Acao = "Tipo Documento: " + tipoDocumento.Nome + "<br/>Tipo Indices: <br/>" + JsonConvert.SerializeObject(indices.Select(x => x.Nome)), DataRegistro = DateTime.UtcNow }, cancellationToken);
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
