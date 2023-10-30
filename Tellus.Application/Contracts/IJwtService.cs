using Tellus.Domain.Models;

namespace Tellus.Application.Contracts
{
    public interface IJwtService
    {
        JsonWebToken GenerateUsuarioAdmToken(UsuarioAdm user);
        JsonWebToken GenerateUsuarioToken(Usuario user);
        JsonWebToken GenerateClienteToken(Cliente cliente);
    }
}
