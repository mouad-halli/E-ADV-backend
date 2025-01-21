using System.Security.Claims;
using Server.common.Exceptions;
using Server.Interfaces.Services;

namespace Server.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string GetUserId()
        {
            var httpContext = _httpContextAccessor.HttpContext;
            
            if (httpContext?.User == null)
                throw new UnauthorizedException("User not authenticated");

            var userId = httpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
                throw new UnauthorizedException("User not authenticated");
                
            return userId;
        }

    }
}