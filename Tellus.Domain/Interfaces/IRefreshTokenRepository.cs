using Tellus.Domain.Models;

namespace Tellus.Domain.Interfaces
{
    public interface IRefreshTokenRepository
    {
        Task<RefreshToken> ObterPorChaveUsuario(string refreshToken);
        Task<RefreshToken> ObterPorChaveCliente(string refreshToken);
        Task AtualizarPorUsuarioId(RefreshToken refreshToken);
        Task AtualizarPorClienteId(RefreshToken refreshToken);
    }
}
