using Application.DTOs;
using Application.Entities.Users.Commands;
using Application.Entities.Users.Handlers;
using Application.Entities.Users.Queries;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using Domain.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibroTests.HandlerTests.UserTests
{
    public class GetUserHandlerTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<ILogger<GetUserHandler>> _loggerMock;
        private readonly GetUserHandler _handler;

        public GetUserHandlerTests()
        {
            _loggerMock = new Mock<ILogger<GetUserHandler>>();
            _mapperMock = new Mock<IMapper>();
            _userRepositoryMock = new Mock<IUserRepository>();

            _handler = new GetUserHandler(
                _userRepositoryMock.Object,
                _mapperMock.Object,
                _loggerMock.Object
            );
        }

        [Fact]
        public async Task Handle_UserDoesNotExist_ReturnsNotFoundObjectResult()
        {
            // Arrange
            var query = new GetUserQuery
            {
                UserId = 1,
            };

            // Set up UserRepository behavior for non-existing user
            _userRepositoryMock
                .Setup(repo => repo.GetUserByIdAsync(query.UserId))
                .ReturnsAsync((User)null);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("User does not exist", (result as NotFoundObjectResult)?.Value);
        }


        [Fact]
        public async Task Handle_GetUser_ReturnsOkObjectResult()
        {
            // Arrange
            var query = new GetUserQuery
            {
                UserId = 1,
            };

            // Set up UserRepository behavior for non-existing user
            _userRepositoryMock
                .Setup(repo => repo.GetUserByIdAsync(query.UserId))
                .ReturnsAsync(new User());

            _mapperMock
                .Setup(m => m.Map<UserDTO>(It.IsAny<User>()))
                .Returns(new UserDTO());
            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            Assert.IsType<UserDTO>((result as OkObjectResult)?.Value);
        }
    }
}
