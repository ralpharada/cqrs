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
    public class AtualizarIndiceHandler : IRequestHandler<AtualizarIndiceQuery, IEvent>
    {
        private readonly IIndiceRepository _repository;
        private readonly IClienteRepository _repositoryCliente;
        private readonly UsuarioAutenticado _usuarioAutenticado;
        private readonly ILogClienteRepository _logClienteRepository;

        public AtualizarIndiceHandler(IIndiceRepository repository, UsuarioAutenticado usuarioAutenticado, IClienteRepository repositoryCliente, ILogClienteRepository logClienteRepository)
        {
            _repository = repository;
            _repositoryCliente = repositoryCliente;
            _usuarioAutenticado = usuarioAutenticado;
            _logClienteRepository = logClienteRepository;
        }
        public async Task<IEvent> Handle(AtualizarIndiceQuery request, CancellationToken cancellationToken)
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
                    var indice = await _repository.ObterPorId(request.Id, cliente.Id, cancellationToken);
                    if (indice != null)
                    {
                        indice.Nome = request.Nome;
                        indice.ETipoIndice = (ETipoIndice)Enum.Parse(typeof(ETipoIndice), request.TipoIndice);
                        indice.Tamanho = request.Tamanho;
                        indice.Mascara = request.Mascara;
                        indice.Obrigatorio = request.Obrigatorio;
                        indice.DataUltimaAlteracao = DateTime.UtcNow;
                        indice.Lista = request.Lista;
                        var result = await _repository.Salvar(indice, cancellationToken);
                        success = result.ModifiedCount > 0;
                        mensagem = success ? "Índice atualizado com sucesso!" : "Nenhuma atualização realizada.";
                        if (success)
                            await _logClienteRepository.Salvar(new LogCliente() { Id = Guid.NewGuid(), Pagina = "Atualizaão do Indice", ClienteId = cliente.Id, Acao = JsonConvert.SerializeObject(indice), DataRegistro = DateTime.UtcNow }, cancellationToken);
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
