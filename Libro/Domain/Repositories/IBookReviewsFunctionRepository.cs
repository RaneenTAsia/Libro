using Domain.Entities;

namespace Domain.Repositories
{
    public interface IBookReviewsFunctionRepository
    {
        Task<List<BookReviewsFunctionResult>> GetBookReviewsAsync(int bookId);
    }
}