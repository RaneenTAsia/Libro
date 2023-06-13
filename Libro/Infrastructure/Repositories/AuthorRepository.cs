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
    public class AuthorRepository : IAuthorRepository
    {
        public LibroDbContext _context = new LibroDbContext();

        public AuthorRepository(LibroDbContext context)
        {
            _context = context;
        }

        public async Task<List<Author>> GetAuthorsByIdsAsync(List<int> authorIds)
        {
            List<Author> authors = new List<Author>();

            for (int i = 0; i < authorIds.Count; i++)
            {
                if (await BookAuthorExists(authorIds[i]))
                    authors.Add(await _context.Authors.FirstOrDefaultAsync(a => a.AuthorId == authorIds[i]));
            }

            return authors;
        }

        public async Task<bool> BookAuthorExists(int authorId)
        {
            return await _context.Authors.AnyAsync(a => a.AuthorId == authorId);
        }
    }
}
