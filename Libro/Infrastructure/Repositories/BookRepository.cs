using AutoDependencyRegistration.Attributes;
using Domain.Entities;
using Domain.Enums;
using Domain.Repositories;

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

        public async Task<Book?> GetBookByIdAsync(int id)
        {
            return await _context.Books.FindAsync(id);
        }

        public async Task<Result> SetBookAsReservedAsync(int bookId)
        {
            var book = await _context.Books.FindAsync(bookId);

            if(book == null)
            {
                return Result.Failed;
            }

            book.BookStatus = (int)Status.Reserved;

            return Result.Completed;
        }

        public async Task<bool> CheckBookIsAvailableAsync(int id)
        {
            var book = await _context.Books.FindAsync(id);

            return book.BookStatus == (int)Status.Available;
        }
    }
}
