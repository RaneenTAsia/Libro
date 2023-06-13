using Domain.Entities;

namespace Domain.Repositories
{
    public interface IBookGenreRepository
    {
        Task<List<BookGenre>> GetGenresByIdsAsync(List<int> genreIds);
    }
}