using Domain.Entities;
using Domain.Enums;

namespace Domain.Repositories
{
    public interface IReviewRepository
    {
        Task<Result> CreateReviewAsync(Review review);
        Task<bool> ReviewExistsAsync(int userId, int bookId);
        Task<Review?> GetReviewAsync(int userId, int bookId);
        Task SaveChangesAsync();
        Task<Review?> DeleteReviewAsync(int userId, int bookId);
    }
}