using MeetingRoomManagement.Entities;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace MeetingRoomManagement.Services
{
    public class TokenServices
    {
        private readonly JwtSettings _jwtSettings;

        public TokenServices(IOptions<JwtSettings> jwtSettings)
        {
            _jwtSettings = jwtSettings.Value
                ?? throw new InvalidOperationException("JwtSettings are not configured.");
        }

        public string GenerateToken(int userId, string role)
        {
            var keyString = _jwtSettings.Key
                ?? throw new InvalidOperationException("JwtSettings:Key is missing.");

            var keyBytes = System.Text.Encoding.UTF8.GetBytes(keyString);
            if (keyBytes.Length < 32)
                throw new InvalidOperationException("Jwt key must be at least 32 bytes (256 bits) for HS256.");

            var creds = new SigningCredentials(new SymmetricSecurityKey(keyBytes), SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
                new Claim(ClaimTypes.Role, role)
            };

            var token = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                notBefore: DateTime.UtcNow,
                expires: DateTime.UtcNow.AddDays(1),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
