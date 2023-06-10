using AutoDependencyRegistration.Attributes;
using Domain.Entities;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    [RegisterClassAsScoped]
    public class ViewOverdueBooksDetailsRepository : IViewOverdueBooksDetailsRepository
    {
        public LibroDbContext _context = new LibroDbContext();

        public ViewOverdueBooksDetailsRepository(LibroDbContext context)
        {
            _context = context;
        }

        public async Task<List<ViewOverdueBookDetails>> GetOverdueBooksAsync()
        {
            return await _context.ViewOverdueBooksDetails.AsNoTracking().ToListAsync();
        }

    }
}
