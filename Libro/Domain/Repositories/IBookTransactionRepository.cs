using Domain.Entities;
using Domain.Enums;

namespace Domain.Repositories
{
    public interface IBookTransactionRepository
    {
        Task<(BookTransaction, Result)> AddBookTransactionAsync(BookTransaction transaction);
        Task<bool> BookTransactionExistsByIdAsync(int transactionId);
        Task<int> BookTransactionCountOfUserById(int userId);
    }
}