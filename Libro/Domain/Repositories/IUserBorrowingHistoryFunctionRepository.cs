using Domain.Entities;

namespace Domain.Repositories
{
    public interface IUserBorrowingHistoryFunctionRepository
    {
        List<UserBorrowingHistoryFunctionResult> GetUserBorrowingHistory(int userId);
    }
}