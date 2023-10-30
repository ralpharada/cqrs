using Flunt.Notifications;
using Tellus.Application.Contracts;
using Tellus.Application.Core;
using Tellus.Application.Queries;
using Tellus.Core.Events;
using Tellus.Domain.Interfaces;
using Tellus.Domain.Models;
using MediatR;

namespace Tellus.Application.Handlers.Autenticacao
{
    public class ValidaUsuarioAdmRefreshTokenHandler : IRequestHandler<ValidaUsuarioAdmRefreshTokenQuery, IEvent>
    {
        private readonly IJwtService _jwtService;
        private readonly IUsuarioAdmRepository _usuarioRepository;
        private readonly IRefreshTokenRepository _refreshTokenRepository;

        private Response _response;

        public ValidaUsuarioAdmRefreshTokenHandler(
            IJwtService jwtService,
            IUsuarioAdmRepository usuarioRepository,
            IRefreshTokenRepository refreshTokenRepository)
        {
            _jwtService = jwtService;
            _usuarioRepository = usuarioRepository;
            _refreshTokenRepository = refreshTokenRepository;
        }

        public async Task<IEvent> Handle(ValidaUsuarioAdmRefreshTokenQuery request, CancellationToken cancellationToken)
        {
            try
            {
                UsuarioAdm usuario = null;

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
                    var jwt = _jwtService.GenerateUsuarioAdmToken(usuario);
                    await _refreshTokenRepository.AtualizarPorUsuarioId(jwt.RefreshToken);

                    return new ResultEvent(true, new
                    {
                        access_token = jwt.AccessToken,
                        refresh_token = jwt.RefreshToken.Token,
                        token_type = jwt.TokenType,
                        expires_in = jwt.ExpiresIn
                    });
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
