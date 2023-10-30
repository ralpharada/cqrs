using Tellus.Application.Contracts;
using Tellus.Application.Crypto;
using Tellus.Application.Queries;
using Tellus.Core.Events;
using Tellus.Domain.Interfaces;
using Tellus.Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Tellus.Application.Handlers.Autenticacao
{
    public class AutenticacaoUsuarioHandler : IRequestHandler<AutenticacaoUsuarioQuery, IEvent>
    {
        private readonly IJwtService _jwtService;
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IUsuarioLogadoRepository _repository;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly IClienteRepository _repositoryCliente;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMediator _mediator;

        public AutenticacaoUsuarioHandler(
            IJwtService jwtService, IUsuarioLogadoRepository repository,
            IUsuarioRepository usuariorRepository,
            IRefreshTokenRepository refreshTokenRepository, IClienteRepository repositoryCliente,
        IHttpContextAccessor httpContextAccessor,
        IMediator mediator)
        {
            _jwtService = jwtService;
            _repository = repository;
            _usuarioRepository = usuariorRepository;
            _refreshTokenRepository = refreshTokenRepository;
            _repositoryCliente = repositoryCliente;
            _httpContextAccessor = httpContextAccessor;
            _mediator = mediator;
        }

        public async Task<IEvent> Handle(AutenticacaoUsuarioQuery request, CancellationToken cancellationToken)
        {
            var mensagem = "";
            try
            {
                Usuario usuario = await _usuarioRepository.ObterPorEmailCadastroAtivo(request.Email, cancellationToken);
                if (usuario == null)
                {
                    mensagem = "Verifique se digitou corretamente os dados de acesso e tente novamente.";
                }
                else
                {
                    var cliente = await _repositoryCliente.ObterPorId(usuario.ClienteId, cancellationToken);
                    if (cliente != null && cliente.Status)
                    {
                        if (!Criptografia.Verify(request.Password, usuario.Senha))
                        {
                            mensagem = "Login/Senha inválidos, tente novamente.";
                        }
                        else
                        {
                            var jwt = _jwtService.GenerateUsuarioToken(usuario);
                            await _refreshTokenRepository.AtualizarPorUsuarioId(jwt.RefreshToken);
                            var ip = _httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();

                            var atualizaLoginHandler =(ResultEvent) await _mediator.Send(new AdicionarUsuarioLogadoQuery(usuario.Id, cliente.Id, ip, cliente.QtdeUsuarios), cancellationToken);
                            if (atualizaLoginHandler.Success)
                            {
                                return new ResultEvent(true, new
                                {
                                    access_token = jwt.AccessToken,
                                    refresh_token = jwt.RefreshToken.Token,
                                    token_type = jwt.TokenType,
                                    expires_in = jwt.ExpiresIn
                                });
                            }
                            else
                            {
                                mensagem = atualizaLoginHandler.Data.ToString();
                            }
                        }
                    }
                    else
                    {
                        mensagem = "Conta destivada.";
                    }
                }
            }
            catch (Exception ex)
            {
                mensagem = ex.Message;
            }
            return new ResultEvent(false, mensagem);
        }
    }
}
