using Application.DTOs;
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
    public class UpdateReviewHandler : IRequestHandler<UpdateReviewCommand, ActionResult>
    {
        public readonly IBookRepository _bookRepository;
        public readonly IReviewRepository _reviewRepository;
        public readonly IMapper _mapper;
        public readonly ILogger<UpdateReviewHandler> _logger;

        public UpdateReviewHandler(IBookRepository bookRepository, IReviewRepository reviewRepository, IMapper mapper, ILogger<UpdateReviewHandler> logger)
        {
            _bookRepository = bookRepository ?? throw new ArgumentNullException();
            _reviewRepository = reviewRepository ?? throw new ArgumentNullException();
            _logger = logger ?? throw new ArgumentNullException();
            _mapper = mapper ?? throw new ArgumentNullException();
        }

        public async Task<ActionResult> Handle(UpdateReviewCommand request, CancellationToken cancellationToken)
        {
            _logger.LogDebug("Check if Book {0} exists", request.BookId);
            if (!(await _bookRepository.BookExistsAsync(request.BookId)))
            {
                _logger.LogDebug("Book does not exist");
                return new NotFoundObjectResult("Book does not exist");
            }

            _logger.LogDebug("Retrieve Review of Book {0} by User {1}", request.BookId, request.UserId);

            var reviewFromRepo = await _reviewRepository.GetReviewAsync(request.UserId, request.BookId);

            if (reviewFromRepo == null)
            {
                _logger.LogDebug("User never reviewed book");
                return new ConflictObjectResult("User never reviewed book");
            }

            var reviewUpdate = _mapper.Map<ReviewForUpdateDTO>(request.CreateReviewDTO);
            reviewUpdate.UserId = request.UserId;
            reviewUpdate.BookId = request.BookId;

            _logger.LogDebug("Update Review of User {0} to Book {1}", request.UserId, request.BookId);
            var reviewUpdated = _mapper.Map(reviewUpdate, reviewFromRepo);

            await _reviewRepository.SaveChangesAsync();

            return new OkObjectResult("Successfully updated Review");
        }
    }
}
