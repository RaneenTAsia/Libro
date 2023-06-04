using Application.Entities.Books.Commands;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Entities.Books.Handlers
{
    public class ReserveBookHandler : IRequestHandler<ReserveBookCommand, (Result, string)>
    {
        public readonly IBookRepository _bookRepository;
        public readonly IBookReservationRepository _bookReservationRepository;
        public readonly ILogger<GetBookDetailsHandler> _logger;
        public readonly IMapper _mapper;

        public ReserveBookHandler(IBookRepository bookRepository, IBookReservationRepository bookReservationRepository, ILogger<GetBookDetailsHandler> logger, IMapper mapper)
        {
            _bookRepository = bookRepository;
            _bookReservationRepository = bookReservationRepository;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<(Result,string)> Handle(ReserveBookCommand request, CancellationToken cancellationToken)
        {
            var isAvailable = await _bookRepository.CheckBookIsAvailableAsync(request.BookId);

            if(!isAvailable)
            {
                return (Result.Failed, "Book not available to reserve");
            }

            await _bookRepository.SetBookAsReservedAsync(request.BookId);

            var reservation = new BookReservation { BookId= request.BookId, UserId = request.UserId, ReserveDate = DateTime.UtcNow };

            var result = await _bookReservationRepository.AddBookReservation(reservation);

            if (result == Result.Failed)
            {
                return (Result.Failed, "Was not able to register reservation");
            }

            return (Result.Completed, "Successfully Reserved Book");
        }
    }
}
