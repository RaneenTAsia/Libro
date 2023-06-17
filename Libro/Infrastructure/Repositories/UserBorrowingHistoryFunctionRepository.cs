using AutoDependencyRegistration.Attributes;
using Domain.Entities;
using Domain.Repositories;

namespace Infrastructure.Repositories
{
    [RegisterClassAsScoped]
    public class UserBorrowingHistoryFunctionRepository : IUserBorrowingHistoryFunctionRepository
    {
        public LibroDbContext _context = new LibroDbContext();

        public UserBorrowingHistoryFunctionRepository(LibroDbContext context)
        {
            _context = context;
        }

        public List<UserBorrowingHistoryFunctionResult> GetUserBorrowingHistory(int userId)
        {
            return _context.UserBorrowingHistoryFunctionResults(userId).ToList();
        }
    }
}
