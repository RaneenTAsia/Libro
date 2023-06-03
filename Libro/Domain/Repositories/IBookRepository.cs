using Domain.Entities;

namespace Application.Abstractions.Repositories
{
    public interface IBookRepository
    {
        Task<Book?> GetBookById(int id);
    }
}