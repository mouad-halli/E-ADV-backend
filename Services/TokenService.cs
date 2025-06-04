using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Server.Interfaces.Services;

namespace Server.Services
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;
        public TokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // generate and return jwt token as a string
        public string CreateToken(List<Claim> claims)
        {
            string key = _configuration["Jwt:key"] ?? throw new InvalidOperationException("jwt key not found");;

            SymmetricSecurityKey SymmetricKey = new(Encoding.UTF8.GetBytes(key));
            SigningCredentials KeyCredentials = new(SymmetricKey, SecurityAlgorithms.HmacSha256);

            JwtSecurityToken jwtToken = new(
                claims: claims,
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Issuer"],
                signingCredentials: KeyCredentials,
                expires: DateTime.UtcNow.AddHours(1)
            );

            return new JwtSecurityTokenHandler().WriteToken(jwtToken);
        }
    }
}