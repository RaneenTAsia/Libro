using Domain.Entities;

namespace Domain.Repositories
{
    public interface IReadingListItemsFunctionRepository
    {
        Task<List<ReadingListItemFunctionResult>> GetReadingListAsync(int readingListId);
    }
}