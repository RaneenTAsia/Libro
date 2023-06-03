using Domain.Entities;

namespace Domain.Repositories
{
    public interface IBookToGenreRepository
    {
        List<BookToBookGenre> GetBookIdsByGenreId(int genreId);
    }
}