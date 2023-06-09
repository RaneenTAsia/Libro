﻿using Domain.Entities;
using Domain.Enums;

namespace Domain.Repositories
{
    public interface IBookRepository
    {
        Task<Book?> GetBookByIdAsync(int id);
        Task<Result> SetBookAsReservedAsync(int bookId);
        Task<bool> CheckBookIsAvailableAsync(int id);
        Task<Status> GetBookStatusByIdAsync(int bookId);
        Task<bool> BookExistsAsync(int bookId);
        Task SaveChangesAsync();
        Task<Result> AddBookAsync(Book book);
        Task<Result> DeleteBookAsync(int bookId);
        Task<List<Book>> GetBooksByIdsAsync(List<int> bookIds);

    }
}