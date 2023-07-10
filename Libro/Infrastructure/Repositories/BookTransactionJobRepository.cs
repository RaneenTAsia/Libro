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
    public class BookTransactionJobRepository : IBookTransactionJobRepository
    {
        public LibroDbContext _context = new LibroDbContext();

        public BookTransactionJobRepository(LibroDbContext context)
        {
            _context = context;
        }

        public async Task<BookTransactionJob?> GetBookTransactionJobAsync(int bookTransactionId)
        {
            return await _context.BookTransactionJobs.FirstOrDefaultAsync(j => j.BookTransactionId == bookTransactionId);
        }

        public async Task<Result> AddBookTransactionJobAsync(BookTransactionJob job)
        {
            if (job != null)
            {
                await _context.BookTransactionJobs.AddAsync(job);
                await _context.SaveChangesAsync();
                return Result.Completed;
            }

            return Result.Failed;
        }

        public async Task<bool> BookTransactionJobExistsAsync(int bookTransactionId)
        {
            return await _context.BookTransactionJobs.AnyAsync(j => j.BookTransactionId == bookTransactionId);
        }

        public void DeleteBookTransactionJob(BookTransactionJob job)
        {
            _context.BookTransactionJobs.Remove(job);
        }
    }
}
