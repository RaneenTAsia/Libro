using Application.Abstractions.Repositories;
using AutoDependencyRegistration.Attributes;
using Domain.Entities;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    [RegisterClassAsScoped]
    public class BookRepository : IBookRepository
    {
        public LibroDbContext _context = new LibroDbContext();

        public BookRepository(LibroDbContext context)
        {
            _context = context;
        }

        public async Task<Book?> GetBookById(int id)
        {
            return await _context.Books.FindAsync(id);
        }
    }
}
