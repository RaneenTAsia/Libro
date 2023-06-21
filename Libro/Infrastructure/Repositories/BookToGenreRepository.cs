using AutoDependencyRegistration.Attributes;
using Domain.Entities;
using Domain.Enums;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    [RegisterClassAsScoped]
    public class BookToGenreRepository : IBookToGenreRepository
    {
        public LibroDbContext _context = new LibroDbContext();

        public BookToGenreRepository(LibroDbContext context)
        {
            _context = context;
        }
        public List<BookToBookGenre> GetBookIdsByGenreId(int genreId)
        {
            return _context.BooksToBookGenres.AsNoTracking().Where(btg => (int)btg.BookGenreId == genreId).ToList();
        }

        public IEnumerable<Genre> GetTop2GenresOfBooks(List<int> bookIds)
        {
            var bookGenres = _context.BooksToBookGenres.Where(bg => bookIds.Any(l => l == bg.BookId)).Select(bg => bg.BookGenreId).ToList();

            var query =  bookGenres.GroupBy(x => x)
                .Where(b => b.Count() > 0)
                .OrderByDescending(b => b.Count())
                .Select(b => b.Key)
                .ToList();

            return query.Take(2);
        }
    }
}
