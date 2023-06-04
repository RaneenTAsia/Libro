using AutoDependencyRegistration.Attributes;
using Domain.Entities;
using Domain.Enums;
using Domain.Repositories;
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
    }
}
