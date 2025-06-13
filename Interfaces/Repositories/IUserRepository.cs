using Server.Models;

namespace Server.Interfaces.Repositories
{
    public interface IUserRepository
    {
        Task<User> GetByEmailAsync(string email);
        Task<User> GetByEmailOrUsernameAsync(string email, string username);
        Task<User> GetByIdAsync(string id);
        Task AddAsync(User user);
        Task<bool> IsEmailTakenAsync(string username);
    }
}