using Domain.Entities;
using Domain.Enums;

namespace Domain.Repositories
{
    public interface IBookReservationRepository
    {
        Task<Result> AddBookReservation(BookReservation bookReservation);
        BookReservation? GetBookReservation(int userId, int bookId);
        Task<Result> DeleteBookReservation(BookReservation reservation);
        Task<bool> BookReservationExistsAsync(int userId, int bookId);
    }
}