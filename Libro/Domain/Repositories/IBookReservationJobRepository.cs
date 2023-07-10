using Domain.Entities;
using Domain.Enums;

namespace Domain.Repositories
{
    public interface IBookReservationJobRepository
    {
        Task<Result> AddBookReservationJobAsync(BookReservationJob job);
        Task<BookReservationJob?> GetBookReservationJobAsync(int bookReservationId, JobType type);
    }
}