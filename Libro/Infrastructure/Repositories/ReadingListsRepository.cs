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
    public class ReadingListsRepository : IReadingListsRepository
    {
        public LibroDbContext _context = new LibroDbContext();

        public ReadingListsRepository(LibroDbContext context)
        {
            _context = context;
        }

        public List<ReadingList> GetReadingListOfUser(int userId)
        {
            return _context.ReadingLists.AsNoTracking().Where(r => r.UserId == userId).ToList();
        }

        public async Task<bool> ReadingListExistsAsync(int readingListId)
        {
            return await _context.ReadingLists.AnyAsync(r => r.ReadingListId == readingListId);
        }

        public async Task<Result> DeleteReadingListAsync(int readingListId)
        {
            var list = await _context.ReadingLists.FirstOrDefaultAsync(r => r.ReadingListId == readingListId);

            if(list == null)
            {
                return Result.Failed;
            }

            _context.ReadingLists.Remove(list);

            await _context.SaveChangesAsync();

            if(await _context.ReadingLists.AnyAsync(r => r.ReadingListId == readingListId)) 
            {
                return Result.Failed;
            }

            return Result.Completed;
        }

        public async Task<Result> AddReadingListAsync(ReadingList readingList)
        {
            await _context.ReadingLists.AddAsync(readingList);
            await _context.SaveChangesAsync();

            if(readingList.ReadingListId == 0)
            {
                return Result.Failed;
            }

            return Result.Completed;
        }
    }
}
