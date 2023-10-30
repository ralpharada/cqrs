using Flunt.Notifications;
using Tellus.Application.Contracts;
using Tellus.Application.Core;
using Tellus.Application.Queries;
using Tellus.Core.Events;
using Tellus.Domain.Interfaces;
using Tellus.Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Tellus.Application.Handlers.Autenticacao
{
    public class ValidaUsuarioRefreshTokenHandler : IRequestHandler<ValidaUsuarioRefreshTokenQuery, IEvent>
    {
        private readonly IJwtService _jwtService;
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly IClienteRepository _repositoryCliente;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMediator _mediator;

        private Response _response;

        public ValidaUsuarioRefreshTokenHandler(
            IJwtService jwtService,
            IUsuarioRepository usuarioRepository,
            IRefreshTokenRepository refreshTokenRepository, IClienteRepository repositoryCliente,
        IHttpContextAccessor httpContextAccessor,
        IMediator mediator)
        {
            _jwtService = jwtService;
            _usuarioRepository = usuarioRepository;
            _refreshTokenRepository = refreshTokenRepository;
            _repositoryCliente = repositoryCliente;
            _httpContextAccessor = httpContextAccessor;
            _mediator = mediator;
        }

        public async Task<IEvent> Handle(ValidaUsuarioRefreshTokenQuery request, CancellationToken cancellationToken)
        {
            try
            {
                Usuario usuario = null;
                if (!String.IsNullOrEmpty(request.RefreshToken))
                {
                    var token = await _refreshTokenRepository.ObterPorChaveUsuario(request.RefreshToken);
                    if (token == null)
                        return new ResultEvent(false, "Refresh Token inválido");
                    else if (token.ExpirationDate < DateTime.Now)
                        return new ResultEvent(false, "Refresh Token expirado");
                    usuario = await _usuarioRepository.ObterPorId(token.UsuarioId.Value, new CancellationToken());

                }
                if (usuario != null)
                {
                    var cliente = await _repositoryCliente.ObterPorId(usuario.ClienteId, cancellationToken);
                    if (cliente != null && cliente.Status)
                    {
                        var jwt = _jwtService.GenerateUsuarioToken(usuario);
                        await _refreshTokenRepository.AtualizarPorUsuarioId(jwt.RefreshToken);
                        var ip = _httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();
                        var atualizaLoginHandler = (ResultEvent)await _mediator.Send(new AtualizarUsuarioLogadoQuery(usuario.Id, ip), cancellationToken);
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
                            return new ResultEvent(false, atualizaLoginHandler.Data.ToString());
                        }
                    }
                    else
                    {
                        return new ResultEvent(false, "Conta destivada.");
                    }
                }
                else
                    return new ResultEvent(false, "Refresh Token expirado");
            }
            catch (Exception ex)
            {
                return new ResultEvent(false, ex.Message);
            }
        }
    }
}
