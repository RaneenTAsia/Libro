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
    }
}
