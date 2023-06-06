using Application.DTOs;
using Application.Entities.Books.Commands;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Entities.Books.Handlers
{
    public class CheckoutBookHandler : IRequestHandler<CheckoutBookCommand, (TransactionToReturnForCheckoutDTO?, string)>
    {
        public readonly IBookRepository _bookRepository;
        public readonly IBookReservationRepository _bookReservationRepository;
        public readonly IBookTransactionRepository _bookTransactionRepository;
        public readonly ILogger<CheckoutBookHandler> _logger;
        public readonly IMapper _mapper;

        public CheckoutBookHandler(IBookRepository bookRepository, IBookReservationRepository bookReservationRepository, IBookTransactionRepository bookTransactionRepository, ILogger<CheckoutBookHandler> logger, IMapper mapper)
        {
            _bookRepository = bookRepository;
            _bookReservationRepository = bookReservationRepository;
            _bookTransactionRepository = bookTransactionRepository;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<(TransactionToReturnForCheckoutDTO?, string)> Handle(CheckoutBookCommand request, CancellationToken cancellationToken)
        {
            var count = await _bookTransactionRepository.BookTransactionCountOfUserById(request.UserId);
            if (count >= 5)
            {
                return (null, "Already have maximum amount of books checked out");
            }

            var bookToCheckout = await _bookRepository.GetBookByIdAsync(request.BookId);
            if (bookToCheckout == null)
            {
                return (null, "Book Doesnt Exist");
            }

            if (!IsAvailableOrReserved(bookToCheckout))
            {
                return (null, "Book Not Available for checkout");
            }

            if(bookToCheckout.BookStatus == (int)Status.Reserved)
            {
                var bookReservation = _bookReservationRepository.GetBookReservation(request.UserId,request.BookId);

                if (bookReservation == null)
                {
                    return (null, "Book is reserved by someone else");
                }
                var deletionResult = await _bookReservationRepository.DeleteBookReservation(bookReservation);
                if (deletionResult == Result.Failed)
                    return (null, "Could not delete reservation");
            }

            BookTransaction transaction;

            if (request.DueDate.HasValue)
                transaction = new BookTransaction { BookId = request.BookId, UserId = request.UserId, BorrowDate = DateTime.UtcNow, DueDate = (DateTime)request.DueDate };
            else
            {
                transaction = new BookTransaction { BookId = request.BookId, UserId = request.UserId, BorrowDate = DateTime.UtcNow, DueDate = DateTime.UtcNow.AddDays(14) };
            }

            var (transactionReturned, result) = await _bookTransactionRepository.AddBookTransactionAsync(transaction);
            
            if(result == Result.Failed)
            {
                return (null,"Could not add transaction");
            }

            bookToCheckout.BookStatus = (int) Status.Borrowed;
            await _bookRepository.SaveChangesAsync();

            if (IsAvailableOrReserved(bookToCheckout))
            {
                return (null, "Book status could not be changed");
            }

            return (_mapper.Map<TransactionToReturnForCheckoutDTO>(transactionReturned), "successfully returned");
        }
        public bool IsAvailableOrReserved(Book book)
        {
            return book.BookStatus == (int)Status.Available || book.BookStatus == (int)Status.Reserved;
        }
    }
}
