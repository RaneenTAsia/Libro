using AutoDependencyRegistration.Attributes;
using Domain.Entities;
using Domain.Enums;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    [RegisterClassAsScoped]
    public class ReviewRepository : IReviewRepository
    {
        public LibroDbContext _context = new LibroDbContext();

        public ReviewRepository(LibroDbContext context)
        {
            _context = context;
        }

        public async Task<bool> ReviewExistsAsync(int userId, int bookId)
        {
            return await _context.Reviews.AnyAsync(r => r.UserId == userId && r.BookId == bookId);
        }

        public async Task<Result> CreateReviewAsync(Review review)
        {
            await _context.Reviews.AddAsync(review);

            await _context.SaveChangesAsync();

            if (review.ReviewId == 0)
            {
                return Result.Failed;
            }

            return Result.Completed;
        }

        public async Task<Review?> GetReviewAsync(int userId, int bookId)
        {
            return await _context.Reviews.FirstOrDefaultAsync(r => r.UserId == userId && r.BookId == bookId);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<Review?> DeleteReviewAsync(int userId, int bookId)
        {
            var review = await GetReviewAsync(userId, bookId);

            if(review == null) 
            {
                return null;

            }

            _context.Reviews.Remove(review);
            await _context.SaveChangesAsync();

            if (await ReviewExistsAsync(userId, bookId))
                return null;

            return review;
        }
    }
}
