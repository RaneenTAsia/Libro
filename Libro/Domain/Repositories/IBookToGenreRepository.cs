using Domain.Entities;
using Domain.Enums;

namespace Domain.Repositories
{
    public interface IBookToGenreRepository
    {
        List<BookToBookGenre> GetBookIdsByGenreId(int genreId);
    }
}