using AutoDependencyRegistration.Attributes;
using Azure.Core;
using Domain.Entities;
using Domain.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    [RegisterClassAsScoped]
    public class ViewBooksRepository : IViewBooksRepository
    {
        public LibroDbContext _context = new LibroDbContext();

        public ViewBooksRepository(LibroDbContext context)
        {
            _context = context;
        }

        public async Task<List<ViewBooks>> GetBooksAsync()
        {
            return await _context.ViewBooks.AsNoTracking().ToListAsync();
        }

        public List<ViewBooks> GetBooksWithAuthor(string author)
        {
            return _context.ViewBooks.AsNoTracking().Where(r => r.Authors.ToLower().Contains(author.ToLower())).ToList();
        }

        public List<ViewBooks> GetBooksWithTitle(string title)
        {
            return _context.ViewBooks.AsNoTracking().Where(r => r.Title.ToLower().Contains(title.ToLower())).ToList();
        }
        public List<ViewBooks> GetBooksWithIds(List<int> bookIds)
        {
            return _context.ViewBooks.AsNoTracking().Where(r => bookIds.Any(b => b == r.BookId)).ToList();
        }
    }
}
