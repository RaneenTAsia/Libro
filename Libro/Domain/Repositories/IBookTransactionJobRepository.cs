using Domain.Entities;
using Domain.Enums;

namespace Domain.Repositories
{
    public interface IBookTransactionJobRepository
    {
        Task<Result> AddBookTransactionJobAsync(BookTransactionJob job);
        Task<bool> BookTransactionJobExistsAsync(int bookTransactionId);
        Task<BookTransactionJob?> GetBookTransactionJobAsync(int bookTransactionId);
        void DeleteBookTransactionJob(BookTransactionJob job);
    }
}