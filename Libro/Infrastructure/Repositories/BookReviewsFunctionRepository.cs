using AutoDependencyRegistration.Attributes;
using Domain.Entities;
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
    public class BookReviewsFunctionRepository : IBookReviewsFunctionRepository
    {
        public LibroDbContext _context = new LibroDbContext();

        public BookReviewsFunctionRepository(LibroDbContext context)
        {
            _context = context;
        }

        public async Task<List<BookReviewsFunctionResult>> GetBookReviewsAsync(int bookId)
        {
            return await _context.BookReviewsFunctionResults(bookId).ToListAsync();
        }
    }
}
