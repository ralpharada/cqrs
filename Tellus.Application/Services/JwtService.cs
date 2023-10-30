using Tellus.Application.Contracts;
using Tellus.Domain.Enums;
using Tellus.Domain.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Tellus.Application.Services
{
    public class JwtService : IJwtService
    {
        public JsonWebToken GenerateUsuarioAdmToken(UsuarioAdm user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(JwtSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Email),
                    new Claim(ClaimTypes.Role,"Admin")
                }),
                Expires = JwtSettings.AccessTokenExpiration,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var securityToken = tokenHandler.CreateToken(tokenDescriptor);
            var accessToken = tokenHandler.WriteToken(securityToken);
            return new JsonWebToken
            {
                AccessToken = accessToken,
                RefreshToken = CreateRefreshTokenUsuario(user.Id, ETipoUsuario.UsuarioAdm),
                ExpiresIn = (long)TimeSpan.FromMinutes(JwtSettings.ValidForMinutes).TotalSeconds
            };
        }
        public JsonWebToken GenerateUsuarioToken(Usuario user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(JwtSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Email),
                    new Claim(ClaimTypes.Role, "User"),
                    new Claim(ClaimTypes.PrimarySid, user.ClienteId.ToString())
                }),
                Expires = JwtSettings.AccessTokenExpiration,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var securityToken = tokenHandler.CreateToken(tokenDescriptor);
            var accessToken = tokenHandler.WriteToken(securityToken);
            return new JsonWebToken
            {
                AccessToken = accessToken,
                RefreshToken = CreateRefreshTokenUsuario(user.Id, ETipoUsuario.Usuario),
                ExpiresIn = (long)TimeSpan.FromMinutes(JwtSettings.ValidForMinutes).TotalSeconds
            };
        }

        public JsonWebToken GenerateClienteToken(Cliente cliente)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(JwtSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, cliente.Email),
                    new Claim(ClaimTypes.Role, "Client")
                }),
                Expires = JwtSettings.AccessTokenExpiration,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var securityToken = tokenHandler.CreateToken(tokenDescriptor);
            var accessToken = tokenHandler.WriteToken(securityToken);
            return new JsonWebToken
            {
                AccessToken = accessToken,
                RefreshToken = CreateRefreshTokenCliente(cliente.Id),
                ExpiresIn = (long)TimeSpan.FromMinutes(JwtSettings.ValidForMinutes).TotalSeconds
            };
        }
        private RefreshToken CreateRefreshTokenUsuario(Guid id, ETipoUsuario eTipoUsuario)
        {
            var refreshToken = new RefreshToken
            {
                UsuarioId = id,
                ExpirationDate = JwtSettings.RefreshTokenExpiration,
                ETipoUsuario = eTipoUsuario
            };

            string token;
            var randomNumber = new byte[32];

            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                token = Convert.ToBase64String(randomNumber);
            }

            refreshToken.Token = token.Replace("+", string.Empty)
                .Replace("=", string.Empty)
                .Replace("/", string.Empty);

            return refreshToken;
        }
        private RefreshToken CreateRefreshTokenCliente(Guid id)
        {
            var refreshToken = new RefreshToken
            {
                ClienteId = id,
                ExpirationDate = JwtSettings.RefreshTokenExpiration
            };

            string token;
            var randomNumber = new byte[32];

            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                token = Convert.ToBase64String(randomNumber);
            }

            refreshToken.Token = token.Replace("+", string.Empty)
                .Replace("=", string.Empty)
                .Replace("/", string.Empty);

            return refreshToken;
        }
    }
}
