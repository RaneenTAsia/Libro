using Application.Entities.Books.Commands;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using Domain.Repositories;
using Domain.Services;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Entities.Books.Handlers
{
    public class ReserveBookHandler : IRequestHandler<ReserveBookCommand, (Result, string)>
    {
        public readonly IBookRepository _bookRepository;
        public readonly IBookReservationRepository _bookReservationRepository;
        public readonly IUserRepository _userRepository;
        public readonly IMailService _mailService;
        public readonly ILogger<ReserveBookHandler> _logger;
        public readonly IMapper _mapper;

        public ReserveBookHandler(IBookRepository bookRepository, IBookReservationRepository bookReservationRepository, IUserRepository userRepository, IMailService mailService, ILogger<ReserveBookHandler> logger, IMapper mapper)
        {
            _bookRepository = bookRepository;
            _bookReservationRepository = bookReservationRepository;
            _userRepository = userRepository;
            _mailService = mailService;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<(Result,string)> Handle(ReserveBookCommand request, CancellationToken cancellationToken)
        {
            _logger.LogDebug("Checking if Book {0} exists", request.BookId);

            var book = await _bookRepository.GetBookByIdAsync(request.BookId);

            if(book == null)
            {
                return (Result.Failed, "Book Does Not Exist");
            }

            var isAvailable = await _bookRepository.CheckBookIsAvailableAsync(request.BookId);

            if(!isAvailable)
            {
                return (Result.Failed, "Book not available to reserve");
            }

            await _bookRepository.SetBookAsReservedAsync(request.BookId);
            await _bookRepository.SaveChangesAsync();

            var reservation = new BookReservation { BookId= request.BookId, UserId = request.UserId, ReserveDate = DateTime.UtcNow };

            _logger.LogDebug("Adding Book Reservation {0}", reservation.BookReservationId);
            var result = await _bookReservationRepository.AddBookReservationAsync(reservation);

            if (result == Result.Failed)
            {
                return (Result.Failed, "Was not able to register reservation");
            }

            //send email for completed reservation
            var user = await _userRepository.GetUserByIdAsync(request.UserId);

            _logger.LogDebug("Sending reservation completion email to {0}", user.Email);
            await _mailService.SendCompletedReservationEmailAsync(user.Email, book.Title);

            return (Result.Completed, "Successfully Reserved Book");
        }
    }
}
