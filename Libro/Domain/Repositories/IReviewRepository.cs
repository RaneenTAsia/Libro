using Domain.Entities;
using Domain.Enums;

namespace Domain.Repositories
{
    public interface IReviewRepository
    {
        Task<Result> CreateReviewAsync(Review review);
        Task<bool> ReviewExistsAsync(int userId, int bookId);
    }
}