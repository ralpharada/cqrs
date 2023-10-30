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
    public class ValidaClienteRefreshTokenHandler : IRequestHandler<ValidaClienteRefreshTokenQuery, IEvent>
    {
        private readonly IJwtService _jwtService;
        private readonly IClienteRepository _clienteRepository;
        private readonly IRefreshTokenRepository _refreshTokenRepository;

        private Response _response;

        public ValidaClienteRefreshTokenHandler(
            IJwtService jwtService,
            IClienteRepository clienteRepository,
            IRefreshTokenRepository refreshTokenRepository)
        {
            _jwtService = jwtService;
            _clienteRepository = clienteRepository;
            _refreshTokenRepository = refreshTokenRepository;
        }

        public async Task<IEvent> Handle(ValidaClienteRefreshTokenQuery request, CancellationToken cancellationToken)
        {
            try
            {
                Cliente cliente = null;
                if (!String.IsNullOrEmpty(request.RefreshToken))
                {
                    var token = await _refreshTokenRepository.ObterPorChaveCliente(request.RefreshToken);
                    if (token == null)
                        return new ResultEvent(false, "Refresh Token inválido");
                    else if (token.ExpirationDate < DateTime.Now)
                        return new ResultEvent(false, "Refresh Token expirado");
                    cliente = await _clienteRepository.ObterPorId(token.ClienteId.Value, new CancellationToken());

                }
                if (cliente != null && cliente.Status)
                {
                    var jwt = _jwtService.GenerateClienteToken(cliente);
                    await _refreshTokenRepository.AtualizarPorClienteId(jwt.RefreshToken);

                    return new ResultEvent(true, new
                    {
                        access_token = jwt.AccessToken,
                        refresh_token = jwt.RefreshToken.Token,
                        token_type = jwt.TokenType,
                        expires_in = jwt.ExpiresIn
                    });
                }
                else
                    return new ResultEvent(false, "Conta desativada.");
            }
            catch (Exception ex)
            {
                return new ResultEvent(false, ex.Message);
            }
        }
    }
}
