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
    public class ReadingItemsRepository : IReadingItemsRepository
    {
        public LibroDbContext _context = new LibroDbContext();

        public ReadingItemsRepository(LibroDbContext context)
        {
            _context = context;
        }

        public async Task<bool> BookExistsInListAsync(int bookId, int readingListId)
        {
            return await _context.ReadingItems.AnyAsync(r => r.BookId == bookId && r.ReadingListId == readingListId);
        }

        public async Task<Result> AddBookToReadingList(int bookId, int readingListId)
        {
            var item = new ReadingItem { ReadingListId = readingListId, BookId = bookId };

            await _context.ReadingItems.AddAsync(item);

            await _context.SaveChangesAsync();

            if (item.ReadingItemId == 0)
            {
                return Result.Failed;
            }

            return Result.Completed;
        }

        public async Task<Result> DeleteBookFromReadingListAsync(int bookId, int readingListId)
        {
            var readingItem = await _context.ReadingItems.FirstOrDefaultAsync(r => r.ReadingListId == readingListId && r.BookId == bookId);
            if (readingItem == null) 
            {
                return Result.Failed;
            }

            _context.ReadingItems.Remove(readingItem);

            await _context.SaveChangesAsync();

            if(await BookExistsInListAsync(bookId, readingListId))
            {
                return Result.Failed;
            }

            return Result.Completed;
        }
    }
}
