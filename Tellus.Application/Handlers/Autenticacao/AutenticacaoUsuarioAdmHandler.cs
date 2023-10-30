using Tellus.Application.Contracts;
using Tellus.Application.Crypto;
using Tellus.Application.Queries;
using Tellus.Core.Events;
using Tellus.Domain.Interfaces;
using Tellus.Domain.Models;
using MediatR;

namespace Tellus.Application.Handlers.Autenticacao
{
    public class AutenticacaoUsuarioAdmHandler : IRequestHandler<AutenticacaoUsuarioAdmQuery, IEvent>
    {
        private readonly IJwtService _jwtService;
        private readonly IUsuarioAdmRepository _usuarioRepository;
        private readonly IRefreshTokenRepository _refreshTokenRepository;

        public AutenticacaoUsuarioAdmHandler(
            IJwtService jwtService,
            IUsuarioAdmRepository usuariorRepository,
            IRefreshTokenRepository refreshTokenRepository)
        {
            _jwtService = jwtService;
            _usuarioRepository = usuariorRepository;
            _refreshTokenRepository = refreshTokenRepository;
        }

        public async Task<IEvent> Handle(AutenticacaoUsuarioAdmQuery request, CancellationToken cancellationToken)
        {
            var mensagem = "";
            try
            {
                UsuarioAdm usuario = await _usuarioRepository.ObterPorEmail(request.Email, cancellationToken);
                if (usuario == null)
                {
                    return new ResultEvent(false, "Verifique se digitou corretamente os dados de acesso e tente novamente.");
                }
                else
                {
                    if (!Criptografia.Verify(request.Password, usuario.Senha))
                    {
                        return new ResultEvent(false, "Login/Senha inválidos, tente novamente.");
                    }
                    else
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
