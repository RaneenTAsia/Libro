using Domain.Entities;
using Domain.Enums;

namespace Domain.Repositories
{
    public interface IUserRepository
    {
        Task<(User, Result)> CreateUserAsync(User user);
        Task<bool> UserExistsByEmailAsync(string email);
        Task<(User?, Result)> ValidateUserCredentialsAsync(string? email, string? Password);
        Task SaveChangesAsync();
        Task<User?> GetUserByIdAsync(int id);
    }
}