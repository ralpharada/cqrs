using Tellus.Application.Contracts;
using Tellus.Application.Core;
using Tellus.Application.Crypto;
using Tellus.Application.Queries;
using Tellus.Core.Events;
using Tellus.Domain.Interfaces;
using Tellus.Domain.Models;
using MediatR;

namespace Tellus.Application.Handlers.Autenticacao
{
    public class AutenticacaoClienteHandler : IRequestHandler<AutenticacaoClienteQuery, IEvent>
    {
        private readonly IJwtService _jwtService;
        private readonly IClienteRepository _clienteRepository;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly ILogClienteRepository _logClienteRepository;
        private Response _response;

        public AutenticacaoClienteHandler(
            IJwtService jwtService,
            IClienteRepository clienterRepository,
            IRefreshTokenRepository refreshTokenRepository,
            ILogClienteRepository logClienteRepository)
        {
            _jwtService = jwtService;
            _clienteRepository = clienterRepository;
            _refreshTokenRepository = refreshTokenRepository;
            _logClienteRepository = logClienteRepository;
        }

        public async Task<IEvent> Handle(AutenticacaoClienteQuery request, CancellationToken cancellationToken)
        {
            try
            {
                Cliente cliente = await _clienteRepository.ObterPorEmailCadastroAtivo(request.Email, cancellationToken);
                if (cliente == null)
                    return new ResultEvent(false, "Verifique se digitou corretamente os dados de acesso e tente novamente");
                else
                {
                    if (!Criptografia.Verify(request.Password, cliente.Senha))
                        return new ResultEvent(false, "Login/Senha inválidos, tente novamente");
                }

                var jwt = _jwtService.GenerateClienteToken(cliente);
                await _refreshTokenRepository.AtualizarPorClienteId(jwt.RefreshToken);
               await _logClienteRepository.Salvar(new LogCliente() { Id = Guid.NewGuid(), Pagina = "login", ClienteId = cliente.Id, Acao = "Autenticação", DataRegistro = DateTime.UtcNow }, cancellationToken);
                return new ResultEvent(true, new
                {
                    access_token = jwt.AccessToken,
                    refresh_token = jwt.RefreshToken.Token,
                    token_type = jwt.TokenType,
                    expires_in = jwt.ExpiresIn
                });
            }
            catch (Exception ex)
            {
                return new ResultEvent(false, ex.Message);
            }
        }
    }
}
