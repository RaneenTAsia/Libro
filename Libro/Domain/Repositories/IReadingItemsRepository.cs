using Domain.Entities;
using Domain.Enums;

namespace Domain.Repositories
{
    public interface IReadingItemsRepository
    {
        Task<Result> AddBookToReadingList(int bookId, int readingListId);
        Task<bool> BookExistsInListAsync(int bookId, int readingListId);
        Task<Result> DeleteBookFromReadingListAsync(int bookId, int readingListId);
    }
}