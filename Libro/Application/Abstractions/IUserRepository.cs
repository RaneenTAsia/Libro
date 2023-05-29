using Application.DTOs;
using Domain.Entities;
using Domain.Enums;

namespace Infrastructure.Repositories
{
    public interface IUserRepository
    {
        Task<(User, Result)> CreateUserAsync(User user);
        Task<bool> UserExistsByEmailAsync(string email);
        Task SaveChangesAsync();
    }
}