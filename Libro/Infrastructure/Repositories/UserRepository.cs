using AutoDependencyRegistration.Attributes;
using Domain.Entities;
using Domain.Enums;
using Infrastructure.Persistence.EntityConfigurations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public async Task<( User, Result)> CreateUserAsync(User user)
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

        public async Task<bool> UserExistsByEmailAsync(string email)
        {
            return await _context.Users.AnyAsync(u => u.Email == email);
        }
    }
}
