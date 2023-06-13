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
    public class BookGenreRepository : IBookGenreRepository
    {
        public LibroDbContext _context = new LibroDbContext();

        public BookGenreRepository(LibroDbContext context)
        {
            _context = context;
        }

        public async Task<List<BookGenre>> GetGenresByIdsAsync(List<int> genreIds)
        {
            List<BookGenre> bookGenres = new List<BookGenre>();

            for (int i = 0; i < genreIds.Count; i++)
            {
                if (await BookGenreExists(genreIds[i]))
                    bookGenres.Add(await _context.BookGenres.FirstOrDefaultAsync(bg => bg.BookGenreId == (Genre)genreIds[i]));
            }

            return bookGenres;
        }

        private async Task<bool> BookGenreExists(int genreId)
        {
            return await _context.BookGenres.AnyAsync(bg => bg.BookGenreId == (Genre)genreId);
        }
    }
}

