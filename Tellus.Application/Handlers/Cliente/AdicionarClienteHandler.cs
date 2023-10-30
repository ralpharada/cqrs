using Tellus.Application.Crypto;
using Tellus.Application.Mapper;
using Tellus.Application.Queries;
using Tellus.Application.Util;
using Tellus.Core.Events;
using Tellus.Domain.Interfaces;
using Tellus.Domain.Models;
using MediatR;
using Microsoft.Extensions.Configuration;
using Tellus.Application.Core;

namespace Tellus.Application.Handlers
{
    public class AdicionarClienteHandler : IRequestHandler<AdicionarClienteQuery, IEvent>
    {
        private readonly UsuarioAutenticado _usuarioAutenticado;
        private readonly IUsuarioAdmRepository _repositoryUsuarioAdm;
        private readonly IClienteRepository _repository;
        private readonly EnviarEmail _enviarEmail;
        private readonly IConfiguration _configuration;
        private readonly IAmazonRepository _repositoryAmazon;

        public AdicionarClienteHandler(IClienteRepository repository, EnviarEmail enviarEmail, IAmazonRepository repositoryAmazon, IConfiguration configuration, UsuarioAutenticado usuarioAutenticado, IUsuarioAdmRepository repositoryUsuarioAdm)
        {
            _repository = repository;
            _enviarEmail = enviarEmail;
            _configuration = configuration;
            _repositoryAmazon = repositoryAmazon;
            _usuarioAutenticado = usuarioAutenticado;
            _repositoryUsuarioAdm = repositoryUsuarioAdm;
        }

        public async Task<IEvent> Handle(AdicionarClienteQuery request, CancellationToken cancellationToken)
        {
            string mensagem = "";
            var success = false;
            Guid id = Guid.Empty;
            try
            {
                if (_usuarioAutenticado.Email == null)
                    return new ResultEvent(success, "Acesso expirado");

                var usuario = await _repositoryUsuarioAdm.ObterPorEmailCadastroAtivo(_usuarioAutenticado.Email, cancellationToken);
                if (usuario != null)
                {
                    if (String.IsNullOrEmpty(request.Senha))
                    {
                        return new ResultEvent(success, "Campo senha é obrigatório");
                    }
                    var cliente = ClienteMapper<Cliente>.Map(request);
                    var hash = Guid.NewGuid().ToString("N");
                    cliente.Id = Guid.NewGuid();
                    cliente.HashAtivacaoCadastro = hash;
                    cliente.DataValidadeAtivacaoCadastro = DateTime.Now.AddHours(24);
                    cliente.HashEsqueciSenha = String.Empty;
                    cliente.DataCadastro = DateTime.UtcNow;
                    cliente.Senha = Criptografia.Encrypt(request.Senha);
                    var result = await _repository.Salvar(cliente, cancellationToken);
                    success = result.UpsertedId > 0;
                    if (success)
                    {
                        var pastaCriada = await _repositoryAmazon.CreateSubfolder(_configuration.GetSection("AmazonS2:ACCESS_KEY_ID").Value, _configuration.GetSection("AmazonS2:SECRET_ACCESS_KEY").Value, _configuration.GetSection("AmazonS2:Bucket").Value, cliente.Id.ToString());
                        if (pastaCriada)
                        {
                            id = cliente.Id;
                            mensagem = "Cadastro efetuado com sucesso!";
                        }
                        else
                        {
                            mensagem = "Falha ao criar o diretório dos arquivos na Amazon!";
                        }
             //           _enviarEmail.Send(cliente.Email, "Seja bem vindo!", "Para ter acesso ao sistema, clique no link abaixo:<br/> <a href=\"https://localhost:5001/portal/api/validarCadastroClienteViaLink/" + hash + "\">https://localhost:5001/portal/api/validarCadastroClienteViaLink/" + hash + "</a>", null);
                    }
                }
            }
            catch (Exception ex)
            {
                mensagem = "Falha ao tentar efetuar o cadastro.";
            }
            return new ResultEvent(success, mensagem,null, id);
        }

    }
}
