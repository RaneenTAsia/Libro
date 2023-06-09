﻿using Application.DTOs;
using Application.Entities.Books.Commands;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using Domain.Repositories;
using Domain.Services;
using Hangfire;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Entities.Books.Handlers
{
    public class ReturnBookHandler : IRequestHandler<ReturnBookCommand, ActionResult>
    {
        public readonly IBookRepository _bookRepository;
        public readonly IBookTransactionRepository _bookTransactionRepository;
        public readonly IBookTransactionJobRepository _bookTransactionJobRepository;
        public readonly ILogger<ReturnBookHandler> _logger;
        public readonly IMapper _mapper;

        public ReturnBookHandler(IBookRepository bookRepository, IBookTransactionRepository bookTransactionRepository, IBookTransactionJobRepository bookTransactionJobRepository, ILogger<ReturnBookHandler> logger, IMapper mapper)
        {
            _bookRepository = bookRepository;
            _bookTransactionRepository = bookTransactionRepository;
            _bookTransactionJobRepository = bookTransactionJobRepository;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<ActionResult> Handle(ReturnBookCommand request, CancellationToken cancellationToken)
        {
            _logger.LogDebug("Checking if Book {0} exists", request.BookId);

            var bookToReturn = await _bookRepository.GetBookByIdAsync(request.BookId);
            if (bookToReturn == null)
            {
                return new NotFoundObjectResult( "Book Doesnt Exist");
            }

            _logger.LogDebug("Checking if Book {0} was ever checked out", request.BookId);

            if (IsAvailableOrReserved(bookToReturn))
            {
                return new NotFoundObjectResult("Book was never checked out");
            }

            var bookTransaction = await _bookTransactionRepository.OngoingBookTransationByBookIdAsync(bookToReturn.BookId);

            if(bookTransaction == null)
            {
                return new ConflictObjectResult("database flawed");
            }

            _logger.LogDebug(" BookTransaction {0} return date before update is {1}", bookTransaction.BookTransactionId, bookTransaction.ReturnDate);

            var bookTransactionJob = await _bookTransactionJobRepository.GetBookTransactionJobAsync(bookTransaction.BookTransactionId);

            if (bookTransactionJob != null)
            {
                BackgroundJob.Delete(bookTransactionJob.JobId);

                _bookTransactionJobRepository.DeleteBookTransactionJob(bookTransactionJob);
            }

            bookTransaction.ReturnDate = DateTime.UtcNow;

            _logger.LogDebug("BookTransaction {0} return date before after is {1}", bookTransaction.BookTransactionId, bookTransaction.ReturnDate);

            bookTransaction.Fine = CalculateFineIfOverdue(bookTransaction);

            _logger.LogDebug("BookTransaction {0} Fine is {1}", bookTransaction.BookTransactionId, bookTransaction.Fine);


            bookToReturn.BookStatus = (int) Status.Available;

            await _bookRepository.SaveChangesAsync();

            if (!IsAvailableOrReserved(bookToReturn))
            {
                return new ConflictObjectResult("Book status could not be changed");
            }

            return new OkObjectResult( _mapper.Map<TransactionToReturnForBookReturnDTO>(bookTransaction));
        }
        public bool IsAvailableOrReserved(Book book)
        {
            return book.BookStatus == (int)Status.Available || book.BookStatus == (int)Status.Reserved;
        }
        public decimal CalculateFineIfOverdue(BookTransaction transaction)
        {
            var daysOverdue = transaction.ReturnDate.Value.Date - transaction.DueDate.Date;

            var fine = 0M;

            if (daysOverdue.Days > 0)
            {
                fine = (decimal)daysOverdue.Days * 0.05M;
            }

            return fine;
        }
    }
}
