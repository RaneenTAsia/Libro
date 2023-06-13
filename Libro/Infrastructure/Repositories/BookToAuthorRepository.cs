using Domain.Entities;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class BookToAuthorRepository
    {
        public LibroDbContext _context = new LibroDbContext();

        public BookToAuthorRepository(LibroDbContext context)
        {
            _context = context;
        }
        public List<BookToBookGenre> GetBookIdsByGenreId(int genreId)
        {
            return _context.BooksToBookGenres.AsNoTracking().Where(btg => (int)btg.BookGenreId == genreId).ToList();
        }

        public async Task<Result> AddAuthorsToBookAsync(int bookId, List<int> authorIds)
        {
            for (int i = 0; i < authorIds.Count; i++)
            {
                await _context.AuthorsToBooks.AddAsync(new AuthorToBook { AuthorId = authorIds[i], BookId = bookId });
            }

            await _context.SaveChangesAsync();

            if (!(await _context.AuthorsToBooks.AnyAsync(btg => btg.BookId == bookId)))
            {
                return Result.Failed;
            }

            return Result.Completed;
        }
    }
}
