using Domain.Entities;
using Domain.Enums;

namespace Domain.Repositories
{
    public interface IReadingListsRepository
    {
        List<ReadingList> GetReadingListOfUser(int userId);
        Task<bool> ReadingListExistsAsync(int readingListId);
        Task<Result> DeleteReadingListAsync(int readingListId);
        Task<Result> AddReadingListAsync(ReadingList readingList);
    }
}