namespace Tellus.Domain.Models
{
    public class JsonWebToken
    {
        public string AccessToken { get; set; } = null!;
        public RefreshToken RefreshToken { get; set; } = null!;
        public string TokenType { get; set; } = "bearer";
        public long ExpiresIn { get; set; }
    }
}
