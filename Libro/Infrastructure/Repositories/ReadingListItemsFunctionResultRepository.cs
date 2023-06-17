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
    public class ReadingListItemsFunctionRepository : IReadingListItemsFunctionRepository
    {
        public LibroDbContext _context = new LibroDbContext();

        public ReadingListItemsFunctionRepository(LibroDbContext context)
        {
            _context = context;
        }

        public async Task<List<ReadingListItemFunctionResult>> GetReadingList(int readingListId)
        {
            return await _context.ReadingListItemsFunctionResults(readingListId).ToListAsync(); ;
        }
    }
}
