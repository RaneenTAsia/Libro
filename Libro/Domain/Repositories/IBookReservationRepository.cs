using Domain.Entities;
using Domain.Enums;

namespace Domain.Repositories
{
    public interface IBookReservationRepository
    {
        Task<Result> AddBookReservation(BookReservation bookReservation);
    }
}