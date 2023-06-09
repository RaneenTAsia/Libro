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

namespace Application.Entities.Books.Handlers
{
    public class CheckoutBookHandler : IRequestHandler<CheckoutBookCommand, ActionResult>
    {
        public readonly IBookRepository _bookRepository;
        public readonly IUserRepository _userRepository;
        public readonly IBookReservationRepository _bookReservationRepository;
        public readonly IBookTransactionRepository _bookTransactionRepository;
        public readonly IBookTransactionJobRepository _bookTransactionJobRepository;
        public readonly IBookReservationJobRepository _bookReservationJobRepository;
        public readonly IMailService _mailService;
        public readonly ILogger<CheckoutBookHandler> _logger;
        public readonly IMapper _mapper;

        public CheckoutBookHandler(IBookRepository bookRepository, IUserRepository userRepository, IBookReservationRepository bookReservationRepository, IBookTransactionRepository bookTransactionRepository, IBookTransactionJobRepository bookTransactionJobRepository, IBookReservationJobRepository bookReservationJobRepository, IMailService mailService, ILogger<CheckoutBookHandler> logger, IMapper mapper)
        {
            _bookRepository = bookRepository;
            _userRepository = userRepository;
            _bookReservationRepository = bookReservationRepository;
            _bookTransactionRepository = bookTransactionRepository;
            _bookTransactionJobRepository = bookTransactionJobRepository;
            _bookReservationJobRepository = bookReservationJobRepository;
            _mailService = mailService;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<ActionResult> Handle(CheckoutBookCommand request, CancellationToken cancellationToken)
        {
            var count = await _bookTransactionRepository.BookTransactionCurrentCountOfUserByIdAsync(request.UserId);
            if (count >= 5)
            {
                return new BadRequestObjectResult( "Already have maximum amount of books checked out");
            }

            _logger.LogDebug("Checking if Book {0} exists", request.BookId);
            var bookToCheckout = await _bookRepository.GetBookByIdAsync(request.BookId);
            if (bookToCheckout == null)
            {
                return new NotFoundObjectResult("Book Doesnt Exist");
            }

            var user = await _userRepository.GetUserByIdAsync(request.UserId);

            if (!IsAvailableOrReserved(bookToCheckout))
            {
                return new BadRequestObjectResult( "Book Not Available for checkout");
            }

            _logger.LogDebug("Checking if Book {0} Is reserved by User {1} exists", request.BookId, request.UserId);
            if (bookToCheckout.BookStatus == (int)Status.Reserved)
            {
                var bookReservation = _bookReservationRepository.GetBookReservation(request.UserId, request.BookId);

                if (bookReservation == null)
                {
                    return new BadRequestObjectResult("Book is reserved by someone else");
                }

                var bookReservationEmailJob = await _bookReservationJobRepository.GetBookReservationJobAsync(bookReservation.BookReservationId, JobType.Email);
                var bookReservationRemovalJob = await _bookReservationJobRepository.GetBookReservationJobAsync(bookReservation.BookReservationId, JobType.Removal);
                var deletionResult = await _bookReservationRepository.DeleteBookReservationAsync(bookReservation);
                if (deletionResult == Result.Failed)
                    return new ConflictObjectResult("Could not delete reservation");

                if (bookReservationRemovalJob != null && bookReservationEmailJob != null)
                {
                    BackgroundJob.Delete(bookReservationRemovalJob.JobId);
                    BackgroundJob.Delete(bookReservationEmailJob.JobId);
                }
            }

            BookTransaction transaction;

            _logger.LogDebug($"Checking if Checkout Request has due date value exists");
            if (request.DueDate.HasValue)
            {
                transaction = new BookTransaction { BookId = request.BookId, UserId = request.UserId, BorrowDate = DateTime.UtcNow, DueDate = (DateTime)request.DueDate };
                _logger.LogDebug("Assigning due date {0} to Transaction", transaction.DueDate);
            }
            else
            {
                transaction = new BookTransaction { BookId = request.BookId, UserId = request.UserId, BorrowDate = DateTime.UtcNow, DueDate = DateTime.UtcNow.AddDays(14) };
                _logger.LogDebug("Assigning due date {0} to Transaction", transaction.DueDate);
            }

            var (transactionReturned, result) = await _bookTransactionRepository.AddBookTransactionAsync(transaction);

            if (result == Result.Failed)
            {
                return new ConflictObjectResult("Could not add transaction");
            }

            var jobId = BackgroundJob.Schedule(
                  () => _mailService.SendOverdueBookEmailAsync(user.Email, bookToCheckout.Title,0M),
                   TimeSpan.FromDays(14));

            var job = new BookTransactionJob { JobId = jobId, BookTransactionId = transaction.BookTransactionId };

            var jobResult = await _bookTransactionJobRepository.AddBookTransactionJobAsync(job);

            bookToCheckout.BookStatus = (int)Status.Borrowed;
            await _bookRepository.SaveChangesAsync();

            if (IsAvailableOrReserved(bookToCheckout))
            {
                return new ConflictObjectResult("Book status could not be changed");
            }

            return new OkObjectResult( _mapper.Map<TransactionToReturnForCheckoutDTO>(transactionReturned));
        }
        public bool IsAvailableOrReserved(Book book)
        {
            return book.BookStatus == (int)Status.Available || book.BookStatus == (int)Status.Reserved;
        }
    }

}

