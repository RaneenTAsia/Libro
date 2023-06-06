using AutoDependencyRegistration.Attributes;
using Domain.Entities;
using Domain.Enums;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    [RegisterClassAsScoped]
    public class BookReservationRepository : IBookReservationRepository
    {
        public LibroDbContext _context = new LibroDbContext();

        public BookReservationRepository(LibroDbContext context)
        {
            _context = context;
        }

        public async Task<Result> AddBookReservation(BookReservation bookReservation)
        {
            _context.BookReservations.Add(bookReservation);

            await _context.SaveChangesAsync();

            if (_context.BookReservations.Any(br => br.BookReservationId == bookReservation.BookReservationId))
            {
                return Result.Completed;
            }

            return Result.Failed;
        }

        public async Task<bool> BookReservationExistsAsync(int userId, int bookId)
        {
            return await _context.BookReservations.AnyAsync(br => br.UserId == userId && br.BookId == bookId);
        }

        public async Task<Result> DeleteBookReservation(BookReservation reservation)
        {
            var deleted = _context.BookReservations.Remove(reservation);
            await _context.SaveChangesAsync();

            if(await BookReservationExistsAsync(reservation.UserId, reservation.BookId))
            {
                return Result.Failed;
            }
            return Result.Completed;
        }

        public BookReservation? GetBookReservation(int userId, int bookId)
        {
            return _context.BookReservations.FirstOrDefault(br => br.UserId == userId && br.BookId == bookId);
        }
    }
}
