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
    public class BookReservationJobRepository : IBookReservationJobRepository
    {
        public LibroDbContext _context = new LibroDbContext();

        public BookReservationJobRepository(LibroDbContext context)
        {
            _context = context;
        }

        public async Task<BookReservationJob?> GetBookReservationJobAsync(int bookReservationId, JobType type)
        {
            return await _context.BookReservationJobs.FirstOrDefaultAsync(j => j.BookReservationId == bookReservationId && j.BookReservationJobType == type);
        }

        public async Task<Result> AddBookReservationJobAsync(BookReservationJob job)
        {
            if (job != null)
            {
                await _context.BookReservationJobs.AddAsync(job);
                await _context.SaveChangesAsync();
                return Result.Completed;
            }

            return Result.Failed;
        }
    }
}
