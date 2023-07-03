using AutoDependencyRegistration.Attributes;
using Domain.Entities;
using Domain.Enums;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
                if (await AuthorExistsAsync(authorIds[i]))
                    authors.Add(await _context.Authors.FirstOrDefaultAsync(a => a.AuthorId == authorIds[i]));
            }

            return authors;
        }

        public async Task<bool> AuthorExistsAsync(int authorId)
        {
            return await _context.Authors.AnyAsync(a => a.AuthorId == authorId);
        }
        
        public async Task<Result> AddAuthorAsync(Author author)
        {
            await _context.Authors.AddAsync(author);

            await _context.SaveChangesAsync();

            if (await _context.Authors.AnyAsync(br => br.AuthorId == author.AuthorId))
            {
                return Result.Completed;
            }

            return Result.Failed;
        }

        public async Task<Author?> GetAuthorByIdAsync(int authorId)
        {
            return await _context.Authors.Include(a => a.WrittenBooks).FirstOrDefaultAsync(a => a.AuthorId == authorId);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<Author?> DeleteAuthorAsync(int authorId)
        {
            var author = await GetAuthorByIdAsync(authorId);

            if (author == null)
            {
                return null;
            }

            _context.Authors.Remove(author);

            return author;
        }
    }
}
