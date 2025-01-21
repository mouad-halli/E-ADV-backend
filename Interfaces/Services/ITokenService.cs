using System.Security.Claims;

namespace Server.Interfaces.Services
{
    public interface ITokenService
    {
        string CreateToken(List<Claim> claims);
    }
}