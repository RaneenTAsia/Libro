using Domain.Entities;
using Domain.Enums;

namespace Domain.Repositories
{
    public interface IAuthorRepository
    {
        Task<List<Author>> GetAuthorsByIdsAsync(List<int> authorIds);
        Task<bool> AuthorExistsAsync(int authorId);
        Task<Result> AddAuthorAsync(Author author);
        Task<Author?> GetAuthorByIdAsync(int authorId);
        Task SaveChangesAsync();
    }
}