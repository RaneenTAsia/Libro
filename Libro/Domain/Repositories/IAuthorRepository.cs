using Domain.Entities;

namespace Domain.Repositories
{
    public interface IAuthorRepository
    {
        Task<List<Author>> GetAuthorsByIdsAsync(List<int> authorIds);
        Task<bool> BookAuthorExists(int authorId);
    }
}