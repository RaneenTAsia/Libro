using AutoDependencyRegistration.Attributes;
using Domain.Entities;
using Domain.Enums;
using Domain.Repositories;
using Infrastructure.Persistence.EntityConfigurations;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    [RegisterClassAsScoped]
    public class UserRepository : IUserRepository
    {
        public LibroDbContext _context = new LibroDbContext();

        public UserRepository(LibroDbContext context)
        {
            _context = context;
        }

        public async Task<(User, Result)> CreateUserAsync(User user)
        {
            if (user == null)
                return (user, Result.Failed);

            await _context.Users.AddAsync(user);

            await _context.SaveChangesAsync();

            if (!await _context.Users.AnyAsync(u => u.UserId == user.UserId))
            {
                return (user, Result.Failed);
            }

            return (user, Result.Completed);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<(User?, Result)> ValidateUserCredentials(string? email, string? Password)
        {
            var context = _context.Users.AsNoTracking();
            var user = await context.FirstOrDefaultAsync(u => u.Email == email);

            if (user == null)
                return (user, Result.Failed);

            var result = PasswordHasher.VerifyPassword(Password, user.PasswordSalt, 3, user.PasswordHash);

            if (result)
                return (user, Result.Completed);

            return (user, Result.Failed);
        }

        public async Task<bool> UserExistsByEmailAsync(string email)
        {
            return await _context.Users.AnyAsync(u => u.Email == email);
        }

        public async Task<User?> GetUserByIdAsync(int id)
        {
            return await _context.Users.FindAsync(id);
        }
    }
}
