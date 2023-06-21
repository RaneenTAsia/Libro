using Domain.Entities;
using Domain.Enums;

namespace Domain.Repositories
{
    public interface IBookReservationRepository
    {
        Task<Result> AddBookReservationAsync(BookReservation bookReservation);
        BookReservation? GetBookReservation(int userId, int bookId);
        Task<Result> DeleteBookReservationAsync(BookReservation reservation);
        Task<bool> BookReservationExistsAsync(int userId, int bookId);
        Task<List<BookReservation>> RemoveBookReservationsAsync();
    }
}