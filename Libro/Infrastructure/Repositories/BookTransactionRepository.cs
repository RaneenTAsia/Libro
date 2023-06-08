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
    }
}
