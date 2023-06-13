using AutoDependencyRegistration.Attributes;
using Domain.Entities;
using Domain.Enums;
using Domain.Repositories;
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

        public async Task<Book?> GetBookByIdAsync(int id)
        {
            return await _context.Books.Include(b => b.BookGenres).Include(b => b.Authors).FirstOrDefaultAsync(b =>b.BookId == id);
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

        public async Task<Status> GetBookStatusByIdAsync(int bookId)
        {
            var book = await _context.Books.FindAsync(bookId);

            return (Status)book.BookStatus;
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<bool> BookExistsAsync(int bookId)
        {
            return await _context.Books.AnyAsync(b => b.BookId == bookId);
        }

        public async Task<Result> AddBookAsync(Book book)
        {
            _context.Books.Add(book);

            await _context.SaveChangesAsync();

            if (await _context.Books.AnyAsync(br => br.BookId == book.BookId))
            {
                return Result.Completed;
            }

            return Result.Failed;
        }
    }
}
