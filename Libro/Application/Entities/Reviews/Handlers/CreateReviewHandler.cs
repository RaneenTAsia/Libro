using Application.Entities.Reviews.Commands;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using Domain.Repositories;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Entities.Reviews.Handlers
{
    public class CreateReviewHandler : IRequestHandler<CreateReviewCommand, ActionResult>
    {
        public readonly IBookRepository _bookRepository;
        public readonly IReviewRepository _reviewRepository;
        public readonly IMapper _mapper;
        public readonly ILogger<CreateReviewHandler> _logger;

        public CreateReviewHandler(IBookRepository bookRepository, IReviewRepository reviewRepository, IMapper mapper, ILogger<CreateReviewHandler> logger)
        {
            _bookRepository = bookRepository ?? throw new ArgumentNullException();
            _reviewRepository= reviewRepository ?? throw new ArgumentNullException();
            _logger = logger ?? throw new ArgumentNullException();
            _mapper = mapper ?? throw new ArgumentNullException();
        }

        public async Task<ActionResult> Handle(CreateReviewCommand request, CancellationToken cancellationToken)
        {
            _logger.LogDebug("Check if Book {0} exists", request.BookId);
            if(!(await _bookRepository.BookExistsAsync(request.BookId)))
            {
                _logger.LogDebug("Book does not exist");
                return new NotFoundObjectResult("Book does not exist");
            }

            _logger.LogDebug("Check if User {0) reviewed Book {1}", request.UserId, request.BookId);
            if(await _reviewRepository.ReviewExistsAsync(request.UserId, request.BookId))
            {
                _logger.LogDebug("User already reviewed book");
                return new ConflictObjectResult("User Already reviewed book");
            }

            var reviewToAdd = _mapper.Map<Review>(request.CreateReviewDTO);
            reviewToAdd.UserId = request.UserId;
            reviewToAdd.BookId = request.BookId;

            _logger.LogDebug("Add Book Review of User {0) to Book {1}", request.UserId, request.BookId);
            var result = await _reviewRepository.CreateReviewAsync(reviewToAdd);

            if(result == Result.Failed)
            {
                return new ConflictObjectResult("Did not add Book Review");
            }

            return new OkObjectResult("Successfulyy Added Review");
        }
    }
}
