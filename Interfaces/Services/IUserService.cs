using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json.Linq;
using Server.Models;

namespace Server.Interfaces.Services
{
    public interface IUserService
    {
        Task<User> getCurrentUser();
        Task<(string? errorMsg, IdentityResult? result)> CreateUser(User newUser, string userPassword);
        Task RegisterUserAsync(string firstname, string lastname, string email, string password);
        Task<(string jwtToken, User user)> LoginAsync(string email, string password);

        Task<(string jwtToken, User user)> MsalLoginAsync(JObject graphUser);
        Task<(string, User)> temporaryLocalUserCreation(JObject graphUser);
        Task<User> findUserById(string userId);
    }
}