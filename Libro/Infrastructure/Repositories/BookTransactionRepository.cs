using AutoDependencyRegistration.Attributes;
using Domain.Entities;
using Domain.Enums;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    [RegisterClassAsScoped]
    public class BookTransactionRepository : IBookTransactionRepository
    {
        public LibroDbContext _context = new LibroDbContext();

        public BookTransactionRepository(LibroDbContext context)
        {
            _context = context;
        }

        public async Task<(BookTransaction, Result)> AddBookTransactionAsync(BookTransaction transaction)
        {
            await _context.BookTransactions.AddAsync(transaction);

            await _context.SaveChangesAsync();

            if (!(await BookTransactionExistsByIdAsync(transaction.BookTransactionId)))
            {
                return (transaction, Result.Failed);
            }
            return (transaction, Result.Completed);
        }

        public async Task<bool> BookTransactionExistsByIdAsync(int transactionId)
        {
            return await _context.BookTransactions.AnyAsync(bt => bt.BookTransactionId == transactionId);
        }

        public async Task<int> BookTransactionCurrentCountOfUserByIdAsync(int userId)
        {
            return await _context.BookTransactions.CountAsync(c => c.UserId == userId && c.ReturnDate == null);
        }

        public async Task<BookTransaction?> OngoingBookTransationByBookIdAsync(int bookId)
        {
            return await _context.BookTransactions.FirstOrDefaultAsync(bt => bt.BookId == bookId && !bt.ReturnDate.HasValue);
        }

        public async Task<List<BookTransaction>> GetOverdueBookTransactionsAsync()
        {
            var overdueBooks = await _context.BookTransactions.Where(b => b.ReturnDate == null && b.DueDate < DateTime.UtcNow).ToListAsync();
            await UpdateFinesAsync(overdueBooks);
            return overdueBooks;
        }

        public async Task UpdateFinesAsync(List<BookTransaction> overdueBooks)
        {

            for (int i = 0; i < overdueBooks.Count; i++)
            {
                var daysOverdue = DateTime.UtcNow.Date - overdueBooks[i].DueDate.Date;

                var fine = 0M;

                if (daysOverdue.Days > 0)
                {
                    fine = (decimal)daysOverdue.Days * 0.05M;
                }

                overdueBooks[i].Fine = fine;
            }
            await _context.SaveChangesAsync();
        }

        public async Task<List<BookTransaction>> GetUserBorrowingHistoryAsync(int userId)
        {
            return await _context.BookTransactions.Where(bt => bt.UserId == userId).ToListAsync();
        }
    }
}
