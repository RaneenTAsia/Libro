using Application.DTOs;
using Application.Entities.Books.Queries;
using AutoMapper;
using Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Entities.Books.Handlers
{
    public class GetBookDetailsHandler : IRequestHandler<GetBookDetailsQuery, BookDetailsDTO>
    {
        public readonly IViewBooksRepository _viewBookRepository;
        public readonly ILogger<GetBookDetailsHandler> _logger;
        public readonly IMapper _mapper;

        public GetBookDetailsHandler(IViewBooksRepository viewBookRepository, ILogger<GetBookDetailsHandler> logger, IMapper mapper)
        {
            _viewBookRepository = viewBookRepository;
            _logger = logger;
            _mapper = mapper;
        }
        public async Task<BookDetailsDTO> Handle(GetBookDetailsQuery request, CancellationToken cancellationToken)
        {
            var resultList = await _viewBookRepository.GetBooksAsync();
            _logger.LogInformation($"Select ViewBooks View");

            var bookToReturn = resultList.FirstOrDefault(r => r.BookId == request.BookId);

            return _mapper.Map<BookDetailsDTO>(bookToReturn);
        }
    }
}
