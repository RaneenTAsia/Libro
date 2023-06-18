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
    public class DeleteReviewHandler : IRequestHandler<DeleteReviewCommand, ActionResult>
    {
        public readonly IBookRepository _bookRepository;
        public readonly IReviewRepository _reviewRepository;
        public readonly IMapper _mapper;
        public readonly ILogger<DeleteReviewHandler> _logger;

        public DeleteReviewHandler(IBookRepository bookRepository, IReviewRepository reviewRepository, IMapper mapper, ILogger<DeleteReviewHandler> logger)
        {
            _bookRepository = bookRepository ?? throw new ArgumentNullException();
            _reviewRepository = reviewRepository ?? throw new ArgumentNullException();
            _logger = logger ?? throw new ArgumentNullException();
            _mapper = mapper ?? throw new ArgumentNullException();
        }

        public async Task<ActionResult> Handle(DeleteReviewCommand request, CancellationToken cancellationToken)
        {
            _logger.LogDebug("Check if Book {0} exists", request.BookId);
            if (!(await _bookRepository.BookExistsAsync(request.BookId)))
            {
                _logger.LogDebug("Book does not exist");
                return new NotFoundObjectResult("Book does not exist");
            }

            _logger.LogDebug("Check if User {0} reviewed Book {1}", request.UserId, request.BookId);
            if (!(await _reviewRepository.ReviewExistsAsync(request.UserId, request.BookId)))
            {
                _logger.LogDebug("User never reviewed book");
                return new ConflictObjectResult("User never reviewed book");
            }

            _logger.LogDebug("Delete Book Review of User {0} to Book {1}", request.UserId, request.BookId);
            var result = await _reviewRepository.DeleteReviewAsync(request.UserId, request.BookId);

            if (result == null)
            {
                return new ConflictObjectResult("Did not delete Book Review");
            }

            return new OkObjectResult("Successfully Deleted Review");
        }
    }
}
